using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using System;
using System.Linq;

namespace IgniteModule.UI
{
    public class IgniteFoldOut : IgniteGUIElementGroup
    {
        const float TweenDuration = 0.3f;

        [SerializeField] Text headerName;
        [SerializeField] Image arrow;
        [SerializeField] RectTransform header;
        [SerializeField] RectTransform view;
        [SerializeField] RectTransform content;
        [SerializeField] Toggle toggle;
        [SerializeField] Image headerImage;
        [SerializeField] VerticalLayoutGroup layoutGroup;
        [SerializeField] ContentSizeFitter sizeFitter;
        [SerializeField] RectTransform headerToggle;
        [SerializeField] HorizontalLayoutGroup headerHorizontalGroup;

        float height;

        public override Transform Content { get { return content.transform; } }

        /// <summary> 子の追加 </summary>
        public override IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            base.Add(element);
            CalcContentHeight();
            return this;
        }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.ElementHeight, size.FontSize, size.Padding.left);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float? height = null, float? fontSize = null, int? indent = null)
        {
            if (height.HasValue)
            {
                header.SetSizeDelta(y: height.Value);
                view.SetAnchoredPosition(y: -height.Value);
                headerName.rectTransform.SetSizeDelta(y: height.Value);
                headerToggle.SetSizeDelta(height.Value, height.Value);
                headerHorizontalGroup.padding.left = (int)height.Value;
            }

            if (fontSize.HasValue)
            {
                headerName.fontSize = (int)fontSize.Value;
            }

            var foldoutParent = Parent as IgniteFoldOut;
            if (foldoutParent != null)
            {
                var padding = foldoutParent.layoutGroup.padding;
                if (indent.HasValue) padding.left += indent.Value;
                this.layoutGroup.padding = padding;
            }
            else
            {
                var padding = Window.Size.Padding;
                this.layoutGroup.padding = new RectOffset(padding.left, padding.right, padding.top, padding.bottom);
            }
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.Font, theme.FoldOutBackground);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? fontColor = null, Color? backgroundColor = null)
        {
            if (fontColor.HasValue)
            {
                headerName.color = fontColor.Value;
                arrow.color = fontColor.Value;
            }
            if (backgroundColor.HasValue) headerImage.color = backgroundColor.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return Observable.Merge(toggle.OnValueChangedAsObservable().AsUnitObservable(), headerImage.OnPointerClickAsObservable().AsUnitObservable());
        }

        /// <summary> 項目を開く </summary>
        public void Open()
        {
            CalcContentHeight();
            toggle.enabled = false;
            TweenUtil.Tween(view, new Vector2(view.sizeDelta.x, height), arrow.rectTransform, Quaternion.Euler(new Vector3(0f, 0f, -90f)), TweenDuration)
                .Subscribe(_ => Window.SetLayout(), () =>
                 {
                     Window.SetLayout();
                     CalcContentHeight();
                     sizeFitter.enabled = true;
                     toggle.enabled = true;
                 })
                 .AddTo(this);

            //// 矢印を回転
            //arrow.rectTransform.DORotate(new Vector3(0f, 0f, -90f), 0.1f).SetEase(Ease.OutQuint);
            //// viewのRectTransformのサイズを変更することで子項目を表示する
            //view.DOSizeDeltaY(height, 0.3f).SetEase(Ease.OutQuint)
            //    .OnStart(() => toggle.enabled = false)  // トグル操作禁止
            //    .OnUpdate(() => Window.SetLayout())  // 動作中Windowのレイアウトを更新する
            //    .OnComplete(() =>
            //    {
            //        Window.SetLayout();
            //        CalcContentHeight();
            //        sizeFitter.enabled = true;
            //        toggle.enabled = true; // トグル操作禁止を解除
            //    });
        }

        /// <summary> 項目を閉じる </summary>
        public void Close()
        {
            sizeFitter.enabled = false;
            toggle.enabled = false;
            TweenUtil.Tween(view, new Vector2(view.sizeDelta.x, 0f), arrow.rectTransform, Quaternion.Euler(Vector3.zero), TweenDuration)
                .Subscribe(_ => Window.SetLayout(), () =>
                {
                    Window.SetLayout();
                    CalcContentHeight();
                    toggle.enabled = true;
                })
                 .AddTo(this);
            //// 矢印を回転
            //arrow.rectTransform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuint);
            //// viewのRectTransformのサイズを変更することで子項目を隠す
            //view.DOSizeDeltaY(0f, 0.3f).SetEase(Ease.OutQuint)
            //    .OnStart(() =>
            //    {
            //        sizeFitter.enabled = false;
            //        toggle.enabled = false;  // トグル操作禁止
            //    })
            //    .OnUpdate(() => Window.SetLayout())  // 動作中Windowのレイアウトを更新する
            //    .OnComplete(() =>
            //    {
            //        Window.SetLayout();
            //        CalcContentHeight();
            //        toggle.enabled = true; // トグル操作禁止を解除
            //    });
        }

        /// <summary> 子要素の高さを計算 </summary>
        void CalcContentHeight()
        {
            sizeFitter.SetLayoutVertical();
            layoutGroup.SetLayoutVertical();
            var contentRect = Content as RectTransform;
            height = contentRect.sizeDelta.y;
        }

        void Init()
        {
            view.SetSizeDelta(y: content.sizeDelta.y);

            // Windowが選択中は一定間隔でレイアウトを更新
            this.UpdateAsObservable().Where(_ => Window.Select.Value).ThrottleFirstFrame(5).SubscribeWithState(this, (_, i) => i.CalcContentHeight());

            // contentの高さが変わったらviewの高さもそれに合わせる
            content.ObserveEveryValueChanged(c => c.sizeDelta.y)
                   .Subscribe(h => view.SetSizeDelta(y: h))
                   .AddTo(this);

            // viewの高さが変わったら自身の高さもそれに合わせる
            view.ObserveEveryValueChanged(v => v.sizeDelta.y)
                .Subscribe(y => RectTransform.SetSizeDelta(y: header.sizeDelta.y + y))
                .AddTo(this);

            // トグル操作で開閉
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

            // Image部分クリックもトグル操作と同様
            headerImage.OnPointerClickAsObservable()
                       .Where(_ => toggle.enabled)
                       .Subscribe(_ => toggle.isOn = !toggle.isOn);

            CalcContentHeight();
        }

        /// <summary> 生成 </summary>
        public static IgniteFoldOut Create(string name, bool isOn = false, Action<IgniteFoldOut> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/FoldOut")).GetComponent<IgniteFoldOut>();

            instance.headerName.text = name;  // 名前を設定
            instance.ID = id;    // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                i.Init();
            });

            instance
            .OnInitializeAfterAsync()
            .DelayFrame(1)
            .SubscribeWithState(instance, (_, i) =>
            {
                // トグルの初期値設定
                i.toggle.isOn = isOn;

                if (!isOn)
                {
                    i.RectTransform.SetSizeDelta(y: i.Window.Size.ElementHeight);
                }
            });

            // 初期化時イベントが設定されていれば呼び出し
            if (onInitialize != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onInitialize, (_, i, onInit) =>
                {
                    onInit(i);
                });
            }

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        /// <summary> 折り畳み項目追加 </summary>
        public static IIgniteGUIGroup AddFoldOut(this IIgniteGUIGroup group, string name, bool isOn = false, Action<IgniteFoldOut> onInitialize = null, string id = "")
        {
            return group.Add(IgniteFoldOut.Create(name, isOn, onInitialize, id));
        }
    }
}