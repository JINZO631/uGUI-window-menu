using System;
using DG.Tweening;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteWindow : UIMonoBehaviour, IIgniteGUIGroup
    {
        const float TweenDuration = 0.3f;

        float height;
        BoolReactiveProperty select = new BoolReactiveProperty(false);
        Subject<IIgniteGUITheme> onSetTheme = new Subject<IIgniteGUITheme>();
        Subject<IIgniteGUISize> onSetSize = new Subject<IIgniteGUISize>();
        readonly Single onIniitalizeAsync = new Single();
        readonly Single onIniitalizeAfterAsync = new Single();

        [SerializeField] Image image;
        [SerializeField] Image headerImage;
        [SerializeField] Text headerName;
        [SerializeField] Image arrowImage;
        [SerializeField] LayoutElement headerLayoutElement;
        [SerializeField] RectTransform headerToggle;
        [SerializeField] RectTransform headerWindowNameRect;
        [SerializeField] HorizontalLayoutGroup headerLayoutGroup;
        [SerializeField] Toggle toggle;
        [SerializeField] Button closeButton;
        [SerializeField] RectTransform arrow;
        [SerializeField] Transform content;
        [SerializeField] VerticalLayoutGroup layoutGroup;
        [SerializeField] GameObject dragArea;
        [SerializeField] Draggable draggable;
        [SerializeField] VariableSizePanel variablePanel;


        /// <summary> Window名 </summary>
        public string HeaderName
        {
            get { return headerName.text; }
            set { headerName.text = value; }
        }

        /// <summary> 親(自身) </summary>
        public IIgniteGUIGroup Parent { get { return null; } }
        /// <summary> 最後に追加された子のGroup </summary>
        public IIgniteGUIGroup LastNestedGroup { get; private set; }
        /// <summary> Window(自身) </summary>
        public IgniteWindow Window { get { return this; } }
        /// <summary> UIのサイズ設定データ </summary>
        public IIgniteGUISize Size { get; private set; }
        /// <summary> UIのテーマ設定データ </summary>
        public IIgniteGUITheme Theme { get; private set; }
        /// <summary> 選択されているか </summary>
        public BoolReactiveProperty Select { get { return select; } }
        /// <summary> 子要素の親Transform </summary>
        public Transform Content { get { return content; } }

        protected override void Start()
        {
            height = RectTransform.sizeDelta.y;

            // 左上の>部分クリックで折り畳み
            toggle.OnValueChangedAsObservable()
                  .Skip(1)
                  .Where(_ => toggle.enabled)
                  .Subscribe(v =>
                  {
                      if (v)
                      {
                          Open();
                      }
                      else
                      {
                          Close();
                      }
                  });

            // タイトル部分ダブルクリックで折り畳み
            headerImage.OnPointerClickAsObservable()
                       .Where(_ => toggle.enabled)
                       .TimeInterval()
                       .Select(t => t.Interval.TotalMilliseconds)
                       .Pairwise()
                       .Where(p => p.Previous > 250)
                       .Where(p => p.Current <= 250)
                       .Subscribe(_ => toggle.isOn = !toggle.isOn);

            // 右上ボタンクリックでwindow閉じ
            closeButton.OnClickAsObservable()
                       .Subscribe(_ => Kill());

            // ドラッグされたら選択状態に
            this.OnBeginDragAsObservable()
                .Subscribe(_ => Select.Value = true);

            // 選択されたらtransformを一番最後にし前面表示されるように
            Select.Where(v => v).Subscribe(_ => Transform.SetAsLastSibling());

            onIniitalizeAsync.Done();
            onIniitalizeAfterAsync.Done();
        }

        /// <summary> 初期化時 </summary>
        public IObservable<Unit> OnInitializeAsync()
        {
            return onIniitalizeAsync;
        }

        /// <summary> 初期化後 </summary>
        public IObservable<Unit> OnInitializeAfterAsync()
        {
            return onIniitalizeAfterAsync;
        }

        /// <summary> サイズ設定イベント </summary>
        public IObservable<IIgniteGUISize> OnSetSize()
        {
            return onSetSize;
        }

        /// <summary> テーマ設定イベント </summary>
        public IObservable<IIgniteGUITheme> OnSetTheme()
        {
            return onSetTheme;
        }

        /// <summary> 子を追加 </summary>
        public IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.SetParent(this);

            element.OnInitializeBeforeAsync()
            .SubscribeWithState2(element, this, (_, e, w) =>
            {
                e.SetSize(w.Size);
                e.SetTheme(w.Theme);
                e.OnSelected().SubscribeWithState(w, (__, window) => window.Select.Value = true).AddTo(w);
                w.OnSetTheme().Subscribe(e.SetTheme);
                w.OnSetSize().Subscribe(e.SetSize);
            });

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                LastNestedGroup = group;
            }

            return this;
        }

        /// <summary> ContentのRectTransformを子のサイズに合わせる </summary>
        public IgniteWindow ContentFit()
        {
            var contentRect = content as RectTransform;
            var children = content.GetChildren().Select(c => c as RectTransform).Where(r => r != null).Select(r => r.sizeDelta.y);
            var height = children.Sum();
            var space = (children.Count() + 1) * layoutGroup.spacing;
            contentRect.SetSizeDelta(y: height + space);
            return this;
        }

        /// <summary> Windowの高さを子のサイズに合わせる(最大: Screen.height) </summary>
        public IgniteWindow Fit()
        {
            ContentFit();
            var contentRect = content as RectTransform;
            var height = System.Math.Min(contentRect.sizeDelta.y + Size.ElementHeight, Screen.height);
            RectTransform.SetSizeDelta(y: height);
            return this;
        }

        /// <summary> ID指定で子を探す </summary>
        public IIgniteGUIElement FindChild(string id)
        {
            return content.GetComponentsInChildren<IIgniteGUIElement>().FirstOrDefault(e => e.ID == id);
        }

        /// <summary> IDと型指定で子を探す </summary>
        public TElement FindChild<TElement>(string id) where TElement : Component, IIgniteGUIElement
        {
            return content.GetComponentsInChildren<TElement>().FirstOrDefault(e => e.ID == id);
        }

        /// <summary> レイアウトの整列 </summary>
        public void SetLayout()
        {
            layoutGroup.SetLayoutVertical();
        }

        /// <summary> Windowを破棄 </summary>
        public void Kill()
        {
            IgniteGUI.ActiveWindow.Remove(this.GetInstanceID());
            Destroy(this.gameObject);
        }

        /// <summary> Windowを閉じる(折り畳み) </summary>
        public void Close()
        {
            // 閉じる前のheightを保存
            height = RectTransform.sizeDelta.y;
            // サイズ変更用のドラッグエリアを非表示
            dragArea.SetActive(false);
            // Windowサイズ変更
            RectTransform.DOSizeDelta(new Vector2(RectTransform.sizeDelta.x, Size.ElementHeight), TweenDuration).SetEase(Ease.OutQuint)
                         .OnStart(() => toggle.enabled = false)
                         .OnComplete(() => toggle.enabled = true);
            // 矢印回転
            arrow.DORotate(Vector3.zero, TweenDuration).SetEase(Ease.OutQuint);
        }

        /// <summary> Windowを開く(折り畳み解除) </summary>
        public void Open()
        {
            // サイズ変更用のドラッグエリアを表示
            dragArea.SetActive(true);
            // Windowサイズ変更
            RectTransform.DOSizeDelta(new Vector2(RectTransform.sizeDelta.x, height), TweenDuration).SetEase(Ease.OutQuint)
                         .OnStart(() => toggle.enabled = false)
                         .OnComplete(() => toggle.enabled = true);
            // 矢印回転
            arrow.DORotate(new Vector3(0f, 0f, -90f), TweenDuration).SetEase(Ease.OutQuint);
        }

        /// <summary> Windowサイズ変更 </summary>
        public IObservable<Unit> OnSizeChanged()
        {
            return variablePanel.OnSizeChanged();
        }

        /// <summary> 指定ウィンドウの右隣に配置 </summary>
        public IgniteWindow SetRightPos(IgniteWindow leftSide)
        {
            OnInitializeAfterAsync()
            .SubscribeWithState2(this, leftSide, (_, i, l) =>
            {
                var pos = l.RectTransform.anchoredPosition;
                pos.x += l.RectTransform.sizeDelta.x;
                i.RectTransform.anchoredPosition = pos;
            });

            return this;
        }

        /// <summary> 指定ウィンドウの左隣に配置 </summary>
        public IgniteWindow SetLeftPos(IgniteWindow rightSide)
        {
            OnInitializeAfterAsync()
            .SubscribeWithState2(this, rightSide, (_, i, r) =>
            {
                var pos = r.RectTransform.anchoredPosition;
                pos.x -= r.RectTransform.sizeDelta.x;
                i.RectTransform.anchoredPosition = pos;
            });

            return this;
        }

        /// <summary> 指定ウィンドウの上隣に配置 </summary>
        public IgniteWindow SetTopPos(IgniteWindow bottomSide)
        {
            OnInitializeAfterAsync()
            .SubscribeWithState2(this, bottomSide, (_, i, b) =>
            {
                var pos = b.RectTransform.anchoredPosition;
                pos.y += b.RectTransform.sizeDelta.y;
                i.RectTransform.anchoredPosition = pos;
            });

            return this;
        }

        /// <summary> 指定ウィンドウの下隣に配置 </summary>
        public IgniteWindow SetBottomPos(IgniteWindow topSide)
        {
            OnInitializeAfterAsync()
            .SubscribeWithState2(this, topSide, (_, i, t) =>
            {
                var pos = t.RectTransform.anchoredPosition;
                pos.y -= t.RectTransform.sizeDelta.y;
                i.RectTransform.anchoredPosition = pos;
            });

            return this;
        }

        /// <summary> 中央に配置 </summary>
        public IgniteWindow SetCenterPos()
        {
            OnInitializeAfterAsync()
            .SubscribeWithState(this, (_, i) =>
            {
                var pos = new Vector2(Screen.width * 0.5f, -Screen.height * 0.5f);
                pos -= new Vector2(i.RectTransform.sizeDelta.x * 0.5f, i.RectTransform.sizeDelta.x * -0.5f);
                i.RectTransform.anchoredPosition = pos;
            });

            return this;
        }

        /// <summary> 左上に配置 </summary>
        public IgniteWindow SetLeftTopPos()
        {
            OnInitializeAfterAsync()
            .SubscribeWithState(this, (_, i) =>
            {
                i.RectTransform.SetAnchoredPosition(0f, 0f);
            });

            return this;
        }

        /// <summary> 右上に配置 </summary>
        public IgniteWindow SetRightTopPos()
        {
            OnInitializeAfterAsync()
            .SubscribeWithState(this, (_, i) =>
            {
                var posX = Screen.width - i.RectTransform.sizeDelta.x;
                i.RectTransform.SetAnchoredPosition(posX, 0f);
            });

            return this;
        }

        /// <summary> 左下に配置 </summary>
        public IgniteWindow SetLeftBottomPos()
        {
            OnInitializeAfterAsync()
            .SubscribeWithState(this, (_, i) =>
            {
                var posY = i.RectTransform.sizeDelta.y - Screen.height;
                i.RectTransform.SetAnchoredPosition(0f, posY);
            });

            return this;
        }

        /// <summary> 右下に配置 </summary>
        public IgniteWindow SetRightBottomPos()
        {
            OnInitializeAfterAsync()
            .SubscribeWithState(this, (_, i) =>
            {
                var posX = Screen.width - i.RectTransform.sizeDelta.x;
                var posY = i.RectTransform.sizeDelta.y - Screen.height;
                i.RectTransform.SetAnchoredPosition(posX, posY);
            });

            return this;
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.WindowHeader, theme.Font, theme.WindowBackground, theme.WindowCloseButton, theme.WindowCloseButtonTransitionColor);
            onSetTheme.OnNext(theme);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? headerColor = null, Color? fontColor = null, Color? backgroundColor = null, Color? closeButtonColor = null, ColorBlock? closeButtonColors = null)
        {
            if (headerColor.HasValue) headerImage.color = headerColor.Value;
            if (fontColor.HasValue)
            {
                headerName.color = fontColor.Value;
                arrowImage.color = fontColor.Value;
            }
            if (backgroundColor.HasValue) image.color = backgroundColor.Value;
            if (closeButtonColor.HasValue) closeButton.targetGraphic.color = closeButtonColor.Value;
            if (closeButtonColors.HasValue) closeButton.colors = closeButtonColors.Value;
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(IIgniteGUISize size)
        {
            SetSize(size.WindowSize, size.FontSize, size.ElementHeight);
            onSetSize.OnNext(size);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(Vector2? windowSize = null, float? fontSize = null, float? headerHeight = null)
        {
            if (windowSize.HasValue) RectTransform.sizeDelta = windowSize.Value;
            if (fontSize.HasValue)
            {
                headerName.fontSize = (int)fontSize.Value;
            }
            if (headerHeight.HasValue)
            {
                headerLayoutElement.minHeight = headerHeight.Value;
                headerToggle.SetSizeDelta(headerHeight.Value, headerHeight.Value);
                headerWindowNameRect.SetSizeDelta(y: headerHeight.Value);
                headerLayoutGroup.padding.left = (int)headerHeight.Value;
                headerLayoutGroup.padding.right = (int)headerHeight.Value;
                closeButton.targetGraphic.rectTransform.SetSizeDelta(headerHeight.Value, headerHeight.Value);
            }
        }

        /// <summary> 生成 </summary>
        public static IgniteWindow Create(string name, Vector2? anchoredPosition = null, Vector2? windowSize = null, bool open = true, bool hideCloseButton = false, bool fixedSize = false, bool fixedPosition = false, IIgniteGUISize size = null, IIgniteGUITheme theme = null, Action<IgniteWindow> onIniitalize = null)
        {
            // 生成
            var window = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Window")).GetComponent<IgniteWindow>();
            // ActiveなWindowとして登録
            IgniteGUI.AddWindow(window.GetInstanceID(), window);
            // 選択時は定期的にcontentサイズに修正をかける
            window.UpdateAsObservable().Where(_ => window.Select.Value).ThrottleFirstFrame(30).Subscribe(_ => window.ContentFit());

            window.gameObject.name = name;                                     // GameObject名
            window.HeaderName = name;                                     // 表示するWindow名
            window.Size = size != null ? size : IgniteGUI.Size;     // 使用するサイズ設定
            window.Theme = theme != null ? theme : IgniteGUI.Theme;  // 使用するテーマ設定


            window.OnInitializeAsync()
            .SubscribeWithState(window, (_, w) =>
            {
                // size
                w.SetSize(w.Size);

                // theme
                w.SetTheme(w.Theme);
            });

            window.OnInitializeAsync()
            .SubscribeWithState(window, (_, w) =>
            {
                // 座標設定
                if (!anchoredPosition.HasValue)
                {
                    IgniteGUI.SetWindowPos(w);
                }
                else
                {
                    w.RectTransform.anchoredPosition = anchoredPosition.Value;
                }

                // サイズ設定
                if (windowSize.HasValue)
                {
                    window.RectTransform.sizeDelta = windowSize.Value;
                }
                else
                {
                    window.RectTransform.sizeDelta = window.Size.WindowSize;
                }
            });

            // 初期折り畳み状態設定
            if (!open)
            {
                window.OnInitializeAsync()
                .SubscribeWithState(window, (_, w) =>
                {
                    w.height = w.RectTransform.sizeDelta.y;
                    w.toggle.isOn = false;
                    w.dragArea.SetActive(false);
                    w.RectTransform.SetSizeDelta(y: w.Size.ElementHeight);
                });
            }

            window.OnInitializeAsync()
            .SubscribeWithState(window, (_, w) =>
            {
                // 閉じるボタンを隠すか
                w.closeButton.gameObject.SetActive(!hideCloseButton);

                // 座標を固定するか
                w.draggable.enabled = !fixedPosition;

                // サイズを固定するか
                w.variablePanel.enabled = !fixedSize;
            });

            // 初期化時処理
            if (onIniitalize != null)
            {
                window.OnInitializeAsync()
                .SubscribeWithState2(window, onIniitalize, (_, w, onInit) =>
                {
                    onInit(w);
                });
            }

            return window;
        }
    }
}