# uGUI-window-menu

uGUIでシンプルなメニューを作成するライブラリ。

デバッグ用途でサクッとGUIを作りたいときに向いてます。

![image](https://github.com/JINZO631/uGUI-window-menu/blob/master/image/ss01.gif)

## 環境

- Unity2017.4 or later
- Scripting Runtime Version .NET 4.x Equivalent

## 導入
[releases](https://github.com/JINZO631/uGUI-window-menu/releases)にあるunitypackageをインポートしてください。

## WebGLのサンプル

https://jinzo631.github.io/uGUI-window-menu/

## 例

コードのみでUIを組み立てることができます。

```csharp
//using IgniteModule;

IgniteWindow.Create("Hello")
            .AddButton("ButtonName", () => 
            {
                // ボタン押したときになにかする
            });
```

```csharp
var window = IgniteWindow.Create("Window");

// 作成したWindowにButtonなどのパーツを足していってUIを作る
window.AddLabel("Label");
window.AddButton("ButtonName", () => { /*OnClick*/});
window.AddToggle((bool value) => { /*OnValueChanged*/});
window.AddSlider((float value) => { /*OnValueChanged*/});
window.AddDropDown((int index) => { /*OnValueChanged*/}, "OptionA", "OptionB", "OptionC");
window.AddInputFieldWithButton("ButtonName", (string inputText) => {/*OnClick*/});

// などなど
```

## ライセンス

- uGUI-window-menu [MIT License](https://github.com/JINZO631/uGUI-window-menu/blob/master/LICENSE)
- SourceHanCodeJP [SIL Open Font License 1.1](https://github.com/adobe-fonts/source-han-code-jp/blob/master/LICENSE.txt)
