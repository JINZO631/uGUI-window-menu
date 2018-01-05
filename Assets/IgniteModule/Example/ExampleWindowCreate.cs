using IgniteModule;
using IgniteModule.UI;
using UnityEngine;

public class ExampleWindowCreate : MonoBehaviour
{
    private void Start()
    {
        IgniteWindow.Create("ExampleWindow", hideCloseButton: false, fixedPosition: false)
                    .AddLabel("Label")
                    .AddButton("Button", _ => Debug.Log("OnClick"))
                    .AddSlider("Slider", v => Debug.LogFormat("Slider:{0}", v))
                    .AddInputField("InputField", onEndEdit: v => Debug.LogFormat("InputField:{0}", v))
                    .AddScroll()
                    .LastNestedGroup
                    .AddLabel("Item1")
                    .AddLabel("Item2")
                    .AddLabel("Item3")
                    .Parent
                    .AddDropdown(Params.To("Item1", "Item2", "Item3"), v => Debug.LogFormat("Dropdown:{0}", v))
                    .AddToggle("Toggle", v => Debug.LogFormat("Toggle:{0}", v))
                    .AddFoldOut("FoldOut")
                    .LastNestedGroup
                    .AddButton("ButtonA", _ => Debug.Log("OnClick ButtonA"))
                    .AddButton("ButtonB", _ => Debug.Log("OnClick ButtonB"))
                    .AddButton("ButtonC", _ => Debug.Log("OnClick ButtonC"));
    }
}