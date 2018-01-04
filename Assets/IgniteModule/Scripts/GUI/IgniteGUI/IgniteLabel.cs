using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UniRx;
using UniRx.Triggers;

namespace IgniteModule.UI
{
    public class IgniteLabel : UIMonoBehaviour, IIgniteGUIElement
    {
        [SerializeField] Image background;
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Color backgroundColor;

        public string ID { get; private set; }
        public IgniteWindow Window { get; private set; }

        public string Text
        {
            get { return text.text; }
            set { text.text = value; }
        }

        protected override void Start()
        {
            background.OnPointerEnterAsObservable().Subscribe(_ => background.color = backgroundColor);

            background.OnPointerExitAsObservable().Subscribe(_ => background.color = Color.clear);
        }

        public void SetSize(IIgniteGUISize size)
        {
            text.fontSize = size.FontSize;
            RectTransform.SetSizeDelta(x: text.rectTransform.sizeDelta.x);
        }

        public void SetTheme(IIgniteGUITheme theme)
        {
            text.color = theme.Font;
            backgroundColor = theme.LabelHighlitedColor;
        }

        public IObservable<Unit> OnSelected()
        {
            return background.OnPointerClickAsObservable().AsUnitObservable();
        }

        public static IgniteLabel Create(IgniteWindow window, string label, IObservable<string> onLabelChagned = null, string id = "")
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Label")).GetComponent<IgniteLabel>();
            instance.Window = window;
            instance.text.text = label;
            if (onLabelChagned != null)
            {
                onLabelChagned.Subscribe(s => instance.Text = s);
            }
            instance.ID = id;
            return instance;
        }
    }

    public static class IgniteLabelExtensions
    {
        public static IIgniteGUIGroup AddLabel(this IIgniteGUIGroup group, string label, IObservable<string> onLabelChagned = null, string id = "")
        {
            return group.Add(IgniteLabel.Create(group.Window, label, onLabelChagned, id));
        }
    }
}