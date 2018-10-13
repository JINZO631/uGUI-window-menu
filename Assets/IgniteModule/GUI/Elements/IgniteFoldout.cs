using UnityEngine;
using UnityEngine.UI;
using IgniteModule.GUICore;
using UnityEngine.EventSystems;
using ATweening;
using System.Collections;

namespace IgniteModule
{
    public class IgniteFoldout : IgniteGUIElementGroup, IPointerClickHandler
    {
        [SerializeField] RectTransform content = null;
        [SerializeField] RectTransform nameRect = null;
        [SerializeField] RectTransform toggleRect = null;
        [SerializeField] RectTransform textRect = null;
        [SerializeField] HorizontalLayoutGroup nameLayoutGroup = null;
        [SerializeField] Text nameText = null;
        [SerializeField] Toggle toggle = null;
        [SerializeField] RectTransform viewRect = null;
        [SerializeField] RectTransform arrowRect = null;
        [SerializeField] ContentSizeFitter sizeFitter = null;
        [SerializeField] VerticalLayoutGroup layoutGroup = null;
        [SerializeField] Image backgroundImage = null;

        public override RectTransform Content => content;

        float height;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!toggle.enabled)
            {
                return;
            }

            onSelected.Invoke();
            toggle.isOn = !toggle.isOn;
        }

        public void Open()
        {
            CalcContentHeight();
            toggle.enabled = false;
            viewRect.DoSizeDeltaY(height, 0.3f)
                    .OnUpdate(() =>
                    {
                        Window.SetLayout();
                    })
                    .OnComplete(() =>
                    {
                        Window.SetLayout();
                        CalcContentHeight();
                        sizeFitter.enabled = true;
                        toggle.enabled = true;
                    });
            RectTransform.DoSizeDeltaY(height + IgniteGUISettings.ElementHeight, 0.3f);
            arrowRect.DoLocalRotate(new Vector3(0f, 0f, -90f), 0.3f)
                    .SetRelative();
        }

        public void Close()
        {
            sizeFitter.enabled = false;
            toggle.enabled = false;
            viewRect.DoSizeDeltaY(0f, 0.3f)
                    .OnUpdate(() => Window.SetLayout())
                    .OnComplete(() =>
                    {
                        Window.SetLayout();
                        CalcContentHeight();
                        toggle.enabled = true;
                    });
            RectTransform.DoSizeDeltaY(IgniteGUISettings.ElementHeight, 0.3f);
            arrowRect.DoLocalRotate(new Vector3(0f, 0f, 90f), 0.3f)
                    .SetRelative();
        }

        void CalcContentHeight()
        {
            sizeFitter.SetLayoutVertical();
            layoutGroup.SetLayoutVertical();
            height = Content.sizeDelta.y;
        }

        void Initialize()
        {
            toggle.onValueChanged.AddListener(v =>
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
        }

        public void SetHeight(float height)
        {
            RectTransform.SetSizeDelta(y: height);
            nameRect.SetSizeDelta(y: height);
            toggleRect.SetSizeDelta(height, height);
            textRect.SetSizeDelta(y: height);
            viewRect.SetAnchoredPosition(y: -height);
            nameLayoutGroup.padding = new RectOffset((int)height, 0, 0, 0);
        }

        public static IgniteFoldout Create(string name)
        {
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Foldout")).GetComponent<IgniteFoldout>();

            instance.backgroundImage.color = IgniteGUISettings.FoldoutColor;
            instance.nameText.text = name;
            instance.nameText.font = IgniteGUISettings.Font;
            instance.nameText.fontSize = IgniteGUISettings.FontSize;
            instance.nameText.color = IgniteGUISettings.FontColor;
            instance.layoutGroup.spacing = IgniteGUISettings.ElementSpacing;
            instance.SetHeight(IgniteGUISettings.ElementHeight);
            instance.Initialize();

            return instance;
        }
    }

    public static partial class IIgniteGUIGroupExtensions
    {
        public static IIgniteGUIGroup AddFoldout(this IIgniteGUIGroup group, string name)
        {
            return group.Add(IgniteFoldout.Create(name));
        }
    }
}