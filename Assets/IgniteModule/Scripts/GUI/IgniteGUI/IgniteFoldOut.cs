using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using TMPro;
using System;
using DG.Tweening;

namespace IgniteModule.UI
{
    public class IgniteFoldOut : IgniteGUIElementGroup
    {
        [SerializeField] TextMeshProUGUI headerName;
        [SerializeField] RectTransform arrow;
        [SerializeField] RectTransform header;
        [SerializeField] RectTransform view;
        [SerializeField] RectTransform content;
        [SerializeField] Toggle toggle;
        [SerializeField] Image headerImage;
        [SerializeField] VerticalLayoutGroup layoutGroup;
        [SerializeField] ContentSizeFitter sizeFitter;

        float height;
        bool tweening = false;

        protected override Transform Content { get { return content.transform; } }

        public override IObservable<Unit> OnSelected()
        {
            return Observable.Merge(toggle.OnValueChangedAsObservable().AsUnitObservable(), headerImage.OnPointerClickAsObservable().AsUnitObservable());
        }

        public override void SetSize(IIgniteGUISize size)
        {
            height = header.sizeDelta.y;
            headerName.fontSize = size.FontSize;
            header.SetSizeDelta(y: size.ElementHeight);
            arrow.SetSizeDelta(y: size.ElementHeight);

            var foldoutParent = Parent as IgniteFoldOut;
            if (foldoutParent != null)
            {
                var padding = foldoutParent.layoutGroup.padding;
                padding.left += size.Padding.left;
                this.layoutGroup.padding = padding;
            }
            else
            {
                this.layoutGroup.padding = size.Padding;
            }
        }

        public override void SetTheme(IIgniteGUITheme theme)
        {
            headerName.color = theme.Font;
            headerImage.color = theme.FoldOutBackground;
        }

        protected override void Start()
        {
            view.SetSizeDelta(y: content.sizeDelta.y);
        }

        void Open()
        {
            arrow.DORotate(new Vector3(0f, 0f, -90f), 0.1f).SetEase(Ease.OutQuint);
            view.DOSizeDeltaY(height, 0.1f).SetEase(Ease.OutQuint)
               .OnStart(() => tweening = true)
               .OnComplete(() =>
               {
                   sizeFitter.enabled = true;
                   Window.LayoutUpdate();
                   tweening = false;
               });

        }

        void Close()
        {
            height = view.sizeDelta.y;
            arrow.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuint);
            view.DOSizeDeltaY(0f, 0.1f).SetEase(Ease.OutQuint)
                .OnStart(() =>
                {
                    sizeFitter.enabled = false;
                    tweening = true;
                })
                .OnComplete(() =>
                {
                    Window.LayoutUpdate();
                    tweening = false;
                });
        }

        void Init()
        {
            content.ObserveEveryValueChanged(c => c.sizeDelta.y)
                   .Subscribe(h => view.SetSizeDelta(y: h));

            view.ObserveEveryValueChanged(v => v.sizeDelta.y)
                .Subscribe(y => RectTransform.SetSizeDelta(y: header.sizeDelta.y + y))
                .AddTo(this);

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
                       .Subscribe(_ => toggle.isOn = !toggle.isOn);
        }

        public static IgniteFoldOut Create(IIgniteGUIGroup parent, IgniteWindow window, string name, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/FoldOut")).GetComponent<IgniteFoldOut>();
            instance.Parent = parent;
            instance.Window = window;
            instance.headerName.text = name;

            instance.Init();

            return instance;
        }
    }

    public static class IgniteFoldOutExtensions
    {
        public static IIgniteGUIGroup AddFoldOut(this IIgniteGUIGroup group, string name, string id = "")
        {
            return group.Add(IgniteFoldOut.Create(group, group.Window, name, id));
        }
    }
}