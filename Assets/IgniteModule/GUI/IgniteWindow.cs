using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using System.Linq;
using UnityEngine.EventSystems;
using ATweening;
using UnityEngine.Events;

namespace IgniteModule
{
    public class IgniteWindow : GUIMonoBehaviour, IIgniteGUIGroup, IBeginDragHandler
    {
        [SerializeField] IgniteWindowHeader header = null;
        [SerializeField] RectTransform content = null;
        [SerializeField] RectTransform scrollRect = null;
        [SerializeField] VerticalLayoutGroup contentLayoutGroup = null;
        [SerializeField] GameObject dragArea = null;
        [SerializeField] DraggableUI draggable = null;
        [SerializeField] VariableSizePanel variableSizePanel = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Image dragAreaImage = null;
        [SerializeField] Image viewportImage = null;

        public IIgniteGUIGroup Parent => null;

        public IIgniteGUIGroup LastNestedGroup { get; private set; }

        public RectTransform Content => content;

        public IgniteWindow Window => this;

        float height;
        bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                if (isSelected != value)
                {
                    if (value)
                    {
                        OnSelect.Invoke();
                    }
                    else
                    {
                        OnDeselect.Invoke();
                    }
                }
                isSelected = value;
            }
        }

        public string Title
        {
            get { return header.Title; }
            set { header.Title = value; }
        }

        public UnityEvent OnInitialize { get; } = new UnityEvent();
        public UnityEvent OnSelect { get; } = new UnityEvent();
        public UnityEvent OnDeselect { get; } = new UnityEvent();

        void Start()
        {
            OnInitialize.Invoke();

            header.OnToggleValueChanged(v =>
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

            header.OnClickKillButton(() => Kill());

            OnSelect.AddListener(() => Transform.SetAsLastSibling());
            variableSizePanel.OnSizeChange.AddListener(sizeDelta => scrollRect.SetSizeDelta(y: sizeDelta.y - IgniteGUISettings.ElementHeight));
            backgroundImage.color = IgniteGUISettings.WindowContentColor;
            dragAreaImage.color = IgniteGUISettings.WindowDragAreaColor;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            IsSelected = true;
        }

        public void Kill()
        {
            IgniteGUI.ActiveWindow.Remove(this.GetInstanceID());
            Destroy(this.gameObject);
        }

        public void Close()
        {
            height = RectTransform.sizeDelta.y;
            dragArea.SetActive(false);
            RectTransform.DoSizeDeltaY(IgniteGUISettings.ElementHeight, 0.3f);
        }

        public void Open()
        {
            // サイズ固定windowかどうかを見て、dragAreaのアクティブを切り替える
            dragArea.SetActive(variableSizePanel.enabled);
            RectTransform.DoSizeDeltaY(height, 0.3f);
        }

        public void Clear()
        {
            foreach (Transform child in Content)
            {
                Destroy(child.gameObject);
            }
        }

        public void SetLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(content);
        }

        public IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.SetParent(this);

            element.OnSelected(() => this.IsSelected = true);

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                LastNestedGroup = group;
            }

            return this;
        }

        public IgniteWindow ContentFit()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(Content);

            var children = Content.GetComponentsInChildren<RectTransform>()
                                .Where(c => c.parent == this.Transform)
                                .Select(c => c.sizeDelta.y)
                                .ToArray();
            var height = children.Sum();
            var space = (children.Length + 1) * contentLayoutGroup.spacing;
            RectTransform.SetSizeDelta(y: height + space);

            LayoutRebuilder.ForceRebuildLayoutImmediate(Content);

            return this;
        }

        public IgniteWindow Fit()
        {
            ContentFit();

            var headerHeight = header.gameObject.activeSelf ? IgniteGUISettings.ElementHeight : 0f;
            var height = Mathf.Min(Content.sizeDelta.y + headerHeight, Screen.height);

            RectTransform.SetSizeDelta(y: height);
            scrollRect.SetSizeDelta(y: height);

            var layouts = Content.GetComponentsInChildren<LayoutGroup>();
            foreach (var i in layouts)
            {
                var layoutGroupRectTransform = i.GetComponent<RectTransform>();

                i.StartCoroutine(IgniteGUIUtility.DelayedAction(() =>
                {
                    LayoutRebuilder.MarkLayoutForRebuild(layoutGroupRectTransform);
                }));
            }

            return this;
        }

        public static IgniteWindow Create(
            string name,
            Vector2? anchoredPosition = null,
            Vector2? windowSize = null,
            bool open = true,
            bool hideCloseButton = false,
            bool hideFoldToggle = false,
            bool hideHeader = false,
            bool viewportRaycast = false,
            bool fixedSize = false,
            bool fixedPosition = false,
            bool stretch = false)
        {
            var window = Instantiate(Resources.Load<GameObject>("IgniteGUI/Window")).GetComponent<IgniteWindow>();

            IgniteGUI.AddWindow(window.GetInstanceID(), window);

            window.gameObject.name = name + "(" + window.GetInstanceID() + ")";
            window.header.SetName(name);
            window.contentLayoutGroup.spacing = IgniteGUISettings.ElementSpacing;
            window.dragArea.GetComponent<RectTransform>().sizeDelta = new Vector2(IgniteGUISettings.ElementHeight, IgniteGUISettings.ElementHeight);
            window.dragAreaImage.rectTransform.sizeDelta = new Vector2(IgniteGUISettings.ElementHeight, IgniteGUISettings.ElementHeight);

            // 座標設定
            if (anchoredPosition.HasValue)
            {
                window.OnInitialize.AddListener(() => window.RectTransform.anchoredPosition = anchoredPosition.Value);
            }
            else
            {
                window.OnInitialize.AddListener(() => IgniteGUI.SetWindowPos(window));
            }

            // サイズ設定
            if (stretch)
            {
                window.OnInitialize.AddListener(() =>
                {
                    window.RectTransform.sizeDelta = Screen.safeArea.size;
                    window.RectTransform.anchoredPosition = Vector2.zero;
                    window.scrollRect.SetSizeDelta(y: window.RectTransform.sizeDelta.y - IgniteGUISettings.ElementHeight);
                });
            }
            else if (windowSize.HasValue)
            {
                window.OnInitialize.AddListener(() =>
                {
                    window.RectTransform.sizeDelta = windowSize.Value;
                    window.scrollRect.SetSizeDelta(y: window.RectTransform.sizeDelta.y - IgniteGUISettings.ElementHeight);
                });
            }
            else
            {
                window.OnInitialize.AddListener(() =>
                {
                    window.RectTransform.sizeDelta = IgniteGUISettings.DefaultWindowSize;
                    window.scrollRect.SetSizeDelta(y: window.RectTransform.sizeDelta.y - IgniteGUISettings.ElementHeight);
                });
            }


            // 初期折りたたみ設定
            if (!open)
            {
                window.OnInitialize.AddListener(() =>
                {
                    window.height = window.RectTransform.sizeDelta.y;
                    window.header.SetToggleValue(false);
                    window.dragArea.SetActive(false);
                    window.RectTransform.SetSizeDelta(y: IgniteGUISettings.ElementHeight);
                });
            }

            // 閉じるボタンを隠すか
            window.header.SetKillButtonActive(!hideCloseButton);

            // 折りたたみボタンを隠すか
            window.header.SetFoldToggleActive(!hideFoldToggle);

            // 座標を固定するか
            window.draggable.enabled = !fixedPosition;

            // サイズを固定するか
            window.variableSizePanel.enabled = !fixedSize;
            window.dragArea.SetActive(!fixedSize);

            // ヘッダーを隠すか
            window.header.gameObject.SetActive(!hideHeader);

            // viewportのImageのRaycastTarget設定(要素部分以外をドラッグしてスクロールできるようにするか)
            window.viewportImage.raycastTarget = viewportRaycast;

            return window;
        }
    }
}