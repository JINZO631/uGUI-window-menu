using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using System;
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
            dragArea.SetActive(true);
            RectTransform.DoSizeDeltaY(height, 0.3f);
        }

        public void SetLayout()
        {
            contentLayoutGroup.SetLayoutVertical();
        }

        public IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.SetParent(this);

            element.OnSelected(() => this.IsSelected = true);

            if (element is IIgniteGUIGroup group)
            {
                LastNestedGroup = group;
            }

            return this;
        }

        public IgniteWindow ContentFit()
        {
            var children = Content.GetComponentsInChildren<RectTransform>()
                                .Where(c => c.parent == this.Transform)
                                .Select(c => c.sizeDelta.y)
                                .ToArray();
            var height = children.Sum();
            var space = (children.Length + 1) * contentLayoutGroup.spacing;
            RectTransform.SetSizeDelta(y: height + space);
            return this;
        }

        public IgniteWindow Fit()
        {
            ContentFit();
            var height = Mathf.Min(Content.sizeDelta.y + IgniteGUISettings.ElementHeight, Screen.height);
            RectTransform.SetSizeDelta(y: height);
            return this;
        }

        public static IgniteWindow Create(string name, Vector2? anchoredPosition = null, Vector2? windowSize = null, bool open = true, bool hideCloseButton = false, bool fixedSize = false, bool fixedPosition = false)
        {
            var window = Instantiate(Resources.Load<GameObject>("IgniteGUI/Window")).GetComponent<IgniteWindow>();

            IgniteGUI.AddWindow(window.GetInstanceID(), window);

            window.gameObject.name = name + "(" + window.GetInstanceID() + ")";
            window.header.SetName(name);

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
            if (windowSize.HasValue)
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

            // 座標を固定するか
            window.draggable.enabled = !fixedPosition;

            // サイズを固定するか
            window.variableSizePanel.enabled = !fixedSize;

            return window;
        }
    }
}