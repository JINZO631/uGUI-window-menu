using DG.Tweening;
using System.Linq;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteWindow : UIMonoBehaviour, IIgniteGUIGroup
    {
        float height;
        bool tweening = false;
        BoolReactiveProperty select = new BoolReactiveProperty(false);

        [SerializeField] Image image;
        [SerializeField] Image headerImage;
        [SerializeField] TextMeshProUGUI headerName;
        [SerializeField] Toggle toggle;
        [SerializeField] Button killButton;
        [SerializeField] RectTransform arrow;
        [SerializeField] Transform content;
        [SerializeField] VerticalLayoutGroup layoutGroup;
        [SerializeField] GameObject dragArea;

        public string HeaderName
        {
            get { return headerName.text; }
            set { headerName.text = value; }
        }

        public IIgniteGUIGroup Parent { get { return null; } }

        public IIgniteGUIGroup LastNestedGroup { get; private set; }

        public IgniteWindow Window { get { return this; } }

        public IIgniteGUISize Size { get; private set; }

        public IIgniteGUITheme Theme { get; private set; }

        public BoolReactiveProperty Select { get { return select; } }


        public IIgniteGUIGroup Add(IIgniteGUIElement element)
        {
            element.RectTransform.SetParent(content);
            element.SetSize(Size);
            element.SetTheme(Theme);

            element.OnSelected().Subscribe(_ => select.Value = true).AddTo(this);

            var group = element as IIgniteGUIGroup;
            if (group != null)
            {
                LastNestedGroup = group;
            }
            return this;
        }

        public IIgniteGUIElement FindChild(string id)
        {
            return content.GetComponentsInChildren<IIgniteGUIElement>().FirstOrDefault(e => e.ID == id);
        }

        public TElement FindChild<TElement>(string id) where TElement : Component, IIgniteGUIElement
        {
            return content.GetComponentsInChildren<TElement>().FirstOrDefault(e => e.ID == id);
        }

        protected override void Start()
        {
            if (Size == null)
            {
                Size = IgniteGUISizeDefault.Instance;
            }
            if (Theme == null)
            {
                Theme = IgniteGUITheme.Default;
            }
            height = RectTransform.sizeDelta.y;

            toggle.OnValueChangedAsObservable()
                  .Where(_ => !tweening)
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

            headerImage.OnPointerClickAsObservable()
                       .TimeInterval()
                       .Select(t => t.Interval.TotalMilliseconds)
                       .Buffer(2, 1)
                       .Where(l => l[0] > 250)
                       .Where(l => l[1] <= 250)
                       .Subscribe(_ => toggle.isOn = !toggle.isOn);

            killButton.OnClickAsObservable()
                      .Subscribe(_ => Kill());

            this.OnBeginDragAsObservable()
                .Subscribe(_ => Select.Value = true);

            Select.Where(v => v).Subscribe(_ => Transform.SetAsLastSibling());
        }

        public void LayoutUpdate()
        {
            layoutGroup.SetLayoutVertical();
        }

        void Kill()
        {
            IgniteGUI.ActiveWindow.Remove(this.GetInstanceID());
            Destroy(this.gameObject);
        }

        void Close()
        {
            // 閉じる前のheightを保存
            height = RectTransform.sizeDelta.y;
            dragArea.SetActive(false);
            RectTransform.DOSizeDelta(new Vector2(RectTransform.sizeDelta.x, Size.HeaderHeight), 0.1f).SetEase(Ease.OutQuint)
                         .OnStart(() => tweening = true)
                         .OnComplete(() => tweening = false);
            arrow.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuint);
        }

        void Open()
        {
            dragArea.SetActive(true);
            RectTransform.DOSizeDelta(new Vector2(RectTransform.sizeDelta.x, height), 0.1f).SetEase(Ease.OutQuint)
                         .OnStart(() => tweening = true)
                         .OnComplete(() => tweening = false);
            arrow.DORotate(new Vector3(0f, 0f, -90f), 0.1f).SetEase(Ease.OutQuint);
        }

        public static IgniteWindow Create(string name, Vector2? anchoredPosition = null, Vector2? size = null)
        {
            var window = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Window")).GetComponent<IgniteWindow>();
            IgniteGUI.AddWindow(window.GetInstanceID(), window);
            window.RectTransform.anchoredPosition = Vector2.zero;
            window.gameObject.name = name;
            window.HeaderName = name;
            window.Size = IgniteGUI.Size;
            window.Theme = IgniteGUI.Theme;

            if (!anchoredPosition.HasValue)
            {
                IgniteGUI.SetWindowPos(window);
            }
            else
            {
                window.RectTransform.anchoredPosition = anchoredPosition.Value;
            }

            if (size.HasValue)
            {
                window.RectTransform.sizeDelta = size.Value;
            }

            return window;
        }
        
    }
}