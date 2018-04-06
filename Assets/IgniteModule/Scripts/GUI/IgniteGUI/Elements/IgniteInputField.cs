using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace IgniteModule.UI
{
    public class IgniteInputField : IgniteGUIElement
    {
        [SerializeField] InputField inputField;
        [SerializeField] Image background;

        /// <summary> InputField </summary>
        public InputField InputField { get { return inputField; } }
        /// <summary> 入力された値 </summary>
        public ReadOnlyReactiveProperty<string> Value { get; private set; }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.InputFieldWidth, size.ElementHeight, size.FontSize);
        }

        /// <summary> サイズ設定 </summary>
        public void SetSize(float? width = null, float? height = null, float? fontSize = null)
        {
            if (width.HasValue) RectTransform.SetSizeDelta(x: width.Value);
            if (height.HasValue) RectTransform.SetSizeDelta(y: height.Value);
            if (fontSize.HasValue)
            {
                InputField.textComponent.fontSize = (int)fontSize.Value;
                var placeholder = InputField.placeholder as Text;
                if (placeholder != null)
                {
                    placeholder.fontSize = (int)fontSize.Value;
                }
            }
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.InputFieldFont, theme.InputFieldBackground, theme.InputFieldTransition);
        }

        /// <summary> テーマ設定 </summary>
        public void SetTheme(Color? fontColor = null, Color? backgroundColor = null, ColorBlock? transitionColors = null)
        {
            if (fontColor.HasValue)
            {
                InputField.textComponent.color = fontColor.Value;
                InputField.placeholder.color = fontColor.Value;
            }

            if (backgroundColor.HasValue) background.color = backgroundColor.Value;
            if (transitionColors.HasValue) InputField.colors = transitionColors.Value;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return inputField.OnPointerClickAsObservable().AsUnitObservable();
        }

        /// <summary> 生成 </summary>
        public static IgniteInputField Create(Action<string> onEndEdit = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false, Action<IgniteInputField> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/InputField")).GetComponent<IgniteInputField>();

            instance.inputField.characterValidation = characterValidation;  // 許容する文字種類
            instance.inputField.readOnly = readOnly;             // 読み取り専用か
            instance.ID = id;                   // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState(instance, (_, i) =>
            {
                // InputFiledの値を元にReactivePropertyを生成
                i.Value = i.inputField.OnValueChangedAsObservable().ToReadOnlyReactiveProperty();
            });

            // 編集終了時処理
            if (onEndEdit != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, onEndEdit, (_, i, endEdit) =>
                {
                    i.InputField.OnEndEditAsObservable().Subscribe(endEdit);
                });
            }

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
        /// <summary> InputField追加 </summary>
        public static IIgniteGUIGroup AddInputField(this IIgniteGUIGroup group, Action<string> onEndEdit = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false, Action<IgniteInputField> onInitialize = null, string id = "")
        {
            return group.Add(IgniteInputField.Create(onEndEdit, characterValidation, readOnly, onInitialize, id));
        }

        /// <summary> InputField追加 </summary>
        public static IIgniteGUIGroup AddInputField(this IIgniteGUIGroup group, string label, Action<string> onEndEdit = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false, Action<IgniteInputField> onInitialize = null, string id = "")
        {
            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).AddInputField(onEndEdit, characterValidation, readOnly, onInitialize, id) as IgniteHorizontalGroup);
        }

        /// <summary> InputFiled追加(ボタン付き) </summary>
        public static IIgniteGUIGroup AddInputFieldWithButton(this IIgniteGUIGroup group, string label, string buttonName, Action<string> onButtonClick, Action<string> onEndEdit = null, InputField.CharacterValidation characterValidation = InputField.CharacterValidation.None, bool readOnly = false, Action<IgniteInputField> onInitialize = null, string id = "")
        {
            var inputField = IgniteInputField.Create(onEndEdit, characterValidation, readOnly, onInitialize, id);
            Action<Unit> buttonClick = _ => onButtonClick(inputField.Value.Value);
            var button = IgniteButton.Create(buttonName, buttonClick);

            return group.Add(IgniteHorizontalGroup.Create().AddLabel(label).Add(inputField).Add(button) as IgniteHorizontalGroup);
        }
    }
}