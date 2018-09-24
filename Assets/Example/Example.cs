using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IgniteModule;

public class Example : MonoBehaviour
{
    void Start()
    {
        IgniteWindow.Create("Window")
                    .AddLabel("Label")
                    .AddButton("CreateWindow", () => Start())
                    .AddButton("ExampleButton", () => Debug.Log("Button Click"))
                    .AddButton("label", "labelbutton", () => Debug.Log("label button"))
                    .AddToggle(v => Debug.Log("toggle: " + v))
                    .AddToggle("togglelabel", v => Debug.Log("label toggle: " + v))
                    .AddToggleWithButton("toggle button", v => Debug.Log("toggle button: " + v))
                    .AddSlider(v => Debug.Log("Slider Value Change" + v))
                    .AddSlider("slider label", v => Debug.Log("label Slider: " + v), -100f, 100f, true)
                    .AddFoldout("Foldout")
                        .LastNestedGroup
                        .AddLabel("Label")
                        .AddButton("Button", () => Debug.Log("OnClick"))
                    .Parent
                    .AddLabel("ToggleGroup")
                    .AddToggleGroup()
                        .LastNestedGroup
                        .AddToggle("Toggle 1", v => Debug.Log("Toggle1: " + v))
                        .AddToggle("Toggle 2", v => Debug.Log("Toggle2: " + v))
                        .AddToggle("Toggle 3", v => Debug.Log("Toggle3: " + v))
                    .Parent
                    .AddDropdown(v => Debug.Log("Dropdown Value: " + v), "option A", "option B", "option C")
                    .AddInputFiled(v => Debug.Log("InputField Value: " + v), v => Debug.Log("InptuField EndEdit:" + v), "初期値", "PlaceHolder")
                    .AddInputFiled("InputField", v => Debug.Log("InputField Value: " + v), v => Debug.Log("InptuField EndEdit:" + v), "初期値", "PlaceHolder")
        ;
    }
}
