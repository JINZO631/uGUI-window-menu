using IgniteModule;
using IgniteModule.UI;
using UnityEngine;
using UniRx;
using System;

public class ExampleWindowCreate : MonoBehaviour
{
    public IgniteGUITheme[] theme;
    public IgniteGUISize[] size;

    private void Start()
    {
        IgniteWindow.Create("ExampleMenu")
            .AddButton("ExampleWindow", _ => ExampleWindow())
            .AddButton("ExampleTheme", _ => ExampleTheme())
            .AddButton("ExampleSize", _ => ExampleSize())
            .AddButton("ExampleSetPos", _ => ExampleSetPos());
    }

    public void ExampleWindow(IIgniteGUITheme theme = null, IIgniteGUISize size = null)
    {
        var window = IgniteWindow.Create("ExampleWindow", windowSize: new Vector2(355f, 540f), theme: theme, size: size);

        // ラベル
        Subject<string> labelChange = new Subject<string>();
        window.AddLabel("Label", labelChange)
              .AddHorizontalGroup(Params.To<IIgniteGUIElement>(IgniteLabel.Create("LabelChange"), IgniteButton.Create("LabelA", _ => labelChange.OnNext("LabelA")), IgniteButton.Create("LabelB", _ => labelChange.OnNext("LabelB"))))
              .AddLabel("xxxx/xx/xx xx:xx:xx", Observable.Interval(TimeSpan.FromSeconds(1)).Select(_ => DateTime.Now.ToString()));

        // ボタン
        window.AddButton("Button", _ => Debug.Log("ButtonClick"))
              .AddButton("Label", "Button", _ => Debug.Log("ButtonClick(Label)"));

        // トグル
        window.AddToggle(v => Debug.LogFormat("Toggle: {0}", v))
            .AddToggle("Toggle", v => Debug.LogFormat("Toggle(Label): {0}", v));

        // スライダー
        BehaviorSubject<float> sliderValue = new BehaviorSubject<float>(0);
        window.AddSlider(onValueChanged: v => Debug.LogFormat("Slider: {0}", v))
            .AddSlider("Slider", onValueChanged: v => Debug.LogFormat("Slider(Label): {0}", v), minValue: -10f, maxValue: 10f)
            .AddSlider("WholeNumbers", v => Debug.LogFormat("Slider(WholeNumbers): {0}", v), minValue: 0f, maxValue: 100f, wholeNumbers: true)
            .AddHorizontalGroup()
            .LastNestedGroup
            .AddLabel("Observable")
            .AddSlider(onValueChanged: v => sliderValue.OnNext(v), observableValue: sliderValue, minValue: -100f, maxValue: 100f)
            .AddButton("Decrement", _ => sliderValue.OnNext(Mathf.Max(sliderValue.Value - 1, -100f)))
            .AddButton("Increment", _ => sliderValue.OnNext(Mathf.Min(sliderValue.Value + 1, 100f)))
            ;

        // ドロップダウン
        var options = new string[] { "Option A", "Option B", "Option C" };
        window.AddLabel("Dropdown")
            .AddDropdown(options, v => Debug.LogFormat("Dropdown: {0}", options[v]));

        // 入力フィールド
        window.AddLabel("InputField")
              .AddInputFieldWithButton("URL:", "Open", v => Application.OpenURL(v), onInitialize: inputField =>
              {
                  inputField.InputField.text = "https://www.youtube.com/channel/UCD-miitqNY3nyukJ4Fnf4_A";
                  inputField.SetSize(width: 200);
              });

        // 折り畳み
        window.AddLabel("FoldOut")
            .AddFoldOut("FoldOut")
            .LastNestedGroup
            .AddLabel("FoldOut Label")
            .AddButton("FoldOut Button", _ => Debug.Log("FoldOut ButtonClick"))
            .AddToggle("FoldOut Toggle", v => Debug.LogFormat("FoldOut Toggle: {0}", v))
            .AddSlider("FoldOut Slider", v => Debug.LogFormat("FoldOut Slider: {0}", v));

        // スクロール
        window.AddLabel("Scroll")
            .AddScroll()
            .LastNestedGroup
            .AddLabel("Scroll Label")
            .AddButton("Scroll Button", _ => Debug.Log("Scroll ButtonClick"))
            .AddToggle("Scroll Toggle", v => Debug.LogFormat("Scroll Toggle: {0}", v))
            .AddSlider("Scroll Slider", v => Debug.LogFormat("Scroll Slider: {0}", v));

        // トグルグループ
        window.AddLabel("ToggleGroup")
            .AddToggleGroup(true)
            .LastNestedGroup
            .AddToggle("Toggle 1", v => Debug.LogFormat("Toggle 1: {0}", v))
            .AddToggle("Toggle 2", v => Debug.LogFormat("Toggle 2: {0}", v))
            .AddToggle("Toggle 3", v => Debug.LogFormat("Toggle 3: {0}", v));

        window.ContentFit();
    }

    public void ExampleTheme()
    {
        var window = 
        IgniteWindow.Create("ExampleTheme");

        for (int i = 0; i < theme.Length; ++i)
        {
            var index = i;
            window.AddButton(string.Format("Theme[{0}]", index), _ => ExampleWindow(theme: theme[index]));
        }
    }

    public void ExampleSize()
    {
        var window =
        IgniteWindow.Create("ExampleSize");

        for (int i = 0; i < size.Length; ++i)
        {
            var index = i;
            window.AddButton(string.Format("Size[{0}]", index), _ => ExampleWindow(size: size[index]));
        }
    }

    public void ExampleSetPos()
    {
        var window = 
        IgniteWindow
            .Create("CenterPos").SetCenterPos();
        window
            .AddButton("Left", _ => IgniteWindow.Create("Left").SetLeftPos(window))
            .AddButton("Right", _ => IgniteWindow.Create("Right").SetRightPos(window))
            .AddButton("Top", _ => IgniteWindow.Create("Top").SetTopPos(window))
            .AddButton("Bottom", _ => IgniteWindow.Create("Bottom").SetBottomPos(window))
            .AddButton("LeftTop", _ => IgniteWindow.Create("LeftTop").SetLeftTopPos())
            .AddButton("RightTop", _ => IgniteWindow.Create("String").SetRightTopPos())
            .AddButton("LeftBottom", _ => IgniteWindow.Create("LeftBottom").SetLeftBottomPos())
            .AddButton("RightBottom", _ => IgniteWindow.Create("RightBottom").SetRightBottomPos());
    }
}