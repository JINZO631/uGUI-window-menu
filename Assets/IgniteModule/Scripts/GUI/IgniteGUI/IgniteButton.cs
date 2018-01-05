using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteButton : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Button button;
        [SerializeField] Image image;

        public IObservable<Unit> OnClick { get { return button.OnClickAsObservable(); } }

        public string ID { get; private set; }

        public IgniteWindow Window { get; private set; }

        public void SetSize(IIgniteGUISize size)
        {
            image.ObserveEveryValueChanged(img => img.rectTransform.sizeDelta.x)
                 .Subscribe(x => RectTransform.SetSizeDelta(x)).AddTo(this);
            RectTransform.SetSizeDelta(image.rectTransform.rect.width, size.ElementHeight);
            image.rectTransform.SetSizeDelta(y: size.ElementHeight);
            text.fontSize = size.FontSize;
        }

        public void SetTheme(IIgniteGUITheme theme)
        {
            button.colors = theme.ButtonTransitionColor;
            image.color = theme.ButtonColor;
            text.color = theme.Font;
        }

        public IObservable<Unit> OnSelected()
        {
            return button.OnClickAsObservable();
        }

        public static IgniteButton Create(IgniteWindow window, string name, Action<Unit> onClick = null, Action<IObservable<Unit>> doSubscribe = null, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Button")).GetComponent<IgniteButton>();
            instance.Window = window;
            instance.text.text = name;
            if (onClick != null)
            {
                instance.button.OnClickAsObservable().Subscribe(onClick);
            }
            if (doSubscribe != null)
            {
                doSubscribe(instance.OnClick);
            }
            instance.ID = id;
            return instance;
        }
    }

    public static class IgniteButtonExtensions
    {
        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group, string buttonName, Action<Unit> onClick = null, Action<IObservable<Unit>> doSubscribe = null, string id = "")
        {
            return group.Add(IgniteButton.Create(group.Window, buttonName, onClick, doSubscribe, id));
        }

        public static IIgniteGUIGroup AddButton(this IIgniteGUIGroup group,string label, string buttonName, Action<Unit> onClick = null, Action<IObservable<Unit>> doSubscribe = null, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create(group).AddButton(buttonName, onClick, doSubscribe, id).AddLabel(label) as IgniteHorizontalGroup);
        }
    }
}