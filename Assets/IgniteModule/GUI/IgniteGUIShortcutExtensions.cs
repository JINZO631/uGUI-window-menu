using System;
using System.Collections.Generic;
using UnityEngine;

namespace IgniteModule
{
    public static partial class IgniteGUIShortcutExtensions
    {
        public static IIgniteGUIGroup AddVector2Field(this IIgniteGUIGroup group, Action<Vector2> onValueChanged = null, Action<Vector2> onEndEdit = null, Vector2? initialValue = null)
        {
            Vector2 vector2 = initialValue ?? Vector2.zero;
            var horizontalGroup = IgniteHorizontalGroup.Create();

            horizontalGroup.AddLabel("x").AddInputField(x =>
            {
                float result;
                if (float.TryParse(x, out result))
                {
                    vector2.x = result;
                    onValueChanged?.Invoke(vector2);
                }
            }, x => onEndEdit?.Invoke(vector2), initialValue: vector2.x.ToString(), placeHolder: "x", width: 100);

            horizontalGroup.AddLabel("y").AddInputField(y =>
            {
                float result;
                if (float.TryParse(y, out result))
                {
                    vector2.y = result;
                    onValueChanged?.Invoke(vector2);
                }
            }, y => onEndEdit?.Invoke(vector2), initialValue: vector2.y.ToString(), placeHolder: "y", width: 100);

            return group.Add(horizontalGroup);
        }

        public static IIgniteGUIGroup AddVector2Field(this IIgniteGUIGroup group, string label, Action<Vector2> onValueChanged = null, Action<Vector2> onEndEdit = null, Vector2? initialValue = null)
        {
            Vector2 vector2 = initialValue ?? Vector2.zero;
            var horizontalGroup = IgniteHorizontalGroup.Create();
            horizontalGroup.AddLabel(label);

            horizontalGroup.AddLabel("x").AddInputField(x =>
            {
                float result;
                if (float.TryParse(x, out result))
                {
                    vector2.x = result;
                    onValueChanged?.Invoke(vector2);
                }
            }, x => onEndEdit?.Invoke(vector2), initialValue: vector2.x.ToString(), placeHolder: "x", width: 100);

            horizontalGroup.AddLabel("y").AddInputField(y =>
            {
                float result;
                if (float.TryParse(y, out result))
                {
                    vector2.y = result;
                    onValueChanged?.Invoke(vector2);
                }
            }, y => onEndEdit?.Invoke(vector2), initialValue: vector2.y.ToString(), placeHolder: "y", width: 100);

            return group.Add(horizontalGroup);
        }

        public static IIgniteGUIGroup AddVector3Field(this IIgniteGUIGroup group, Action<Vector3> onValueChanged = null, Action<Vector3> onEndEdit = null, Vector3? initialValue = null)
        {
            Vector3 vector3 = initialValue ?? Vector3.zero;
            var horizontalGroup = IgniteHorizontalGroup.Create();

            horizontalGroup.AddLabel("x").AddInputField(x =>
            {
                float result;
                if (float.TryParse(x, out result))
                {
                    vector3.x = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, x => onEndEdit?.Invoke(vector3), initialValue: vector3.x.ToString(), placeHolder: "x", width: 100);

            horizontalGroup.AddLabel("y").AddInputField(y =>
            {
                float result;
                if (float.TryParse(y, out result))
                {
                    vector3.y = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, y => onEndEdit?.Invoke(vector3), initialValue: vector3.y.ToString(), placeHolder: "y", width: 100);

            horizontalGroup.AddLabel("z").AddInputField(z =>
            {
                float result;
                if (float.TryParse(z, out result))
                {
                    vector3.z = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, z => onEndEdit?.Invoke(vector3), initialValue: vector3.z.ToString(), placeHolder: "z", width: 100);

            return group.Add(horizontalGroup);
        }

        public static IIgniteGUIGroup AddVector3Field(this IIgniteGUIGroup group, string label = null, Action<Vector3> onValueChanged = null, Action<Vector3> onEndEdit = null, Vector3? initialValue = null)
        {
            Vector3 vector3 = initialValue ?? Vector3.zero;
            var horizontalGroup = IgniteHorizontalGroup.Create();
            if (label != null)
            {
                horizontalGroup.AddLabel(label);
            }
            horizontalGroup.AddLabel("x").AddInputField(x =>
            {
                float result;
                if (float.TryParse(x, out result))
                {
                    vector3.x = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, x => onEndEdit?.Invoke(vector3), initialValue: vector3.x.ToString(), placeHolder: "x", width: 100);

            horizontalGroup.AddLabel("y").AddInputField(y =>
            {
                float result;
                if (float.TryParse(y, out result))
                {
                    vector3.y = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, y => onEndEdit?.Invoke(vector3), initialValue: vector3.y.ToString(), placeHolder: "y", width: 100);

            horizontalGroup.AddLabel("z").AddInputField(z =>
            {
                float result;
                if (float.TryParse(z, out result))
                {
                    vector3.z = result;
                    onValueChanged?.Invoke(vector3);
                }
            }, z => onEndEdit?.Invoke(vector3), initialValue: vector3.z.ToString(), placeHolder: "z", width: 100);

            return group.Add(horizontalGroup);
        }

        public static IIgniteGUIGroup AddEnumDropdown<TEnum>(this IIgniteGUIGroup group, Action<int> onValueChanged)
        {
            return group.AddDropdown(onValueChanged, Enum.GetNames(typeof(TEnum)));
        }

        public static IIgniteGUIGroup AddRGBSlider(this IIgniteGUIGroup group, Action<Color> onValueChanged, Color? initialValue = null)
        {
            var color = initialValue ?? Color.white;
            return group
            .AddSlider("R", r =>
            {
                color.r = r;
                onValueChanged(color);
            }, initialValue: color.r)
            .AddSlider("G", g =>
            {
                color.g = g;
                onValueChanged(color);
            }, initialValue: color.g)
            .AddSlider("B", b =>
            {
                color.b = b;
                onValueChanged(color);
            }, initialValue: color.b);
        }

        public static IIgniteGUIGroup AddRGBASlider(this IIgniteGUIGroup group, Action<Color> onValueChanged, Color? initialValue = null)
        {
            var color = initialValue ?? Color.white;
            return group
            .AddSlider("R", r =>
            {
                color.r = r;
                onValueChanged(color);
            }, initialValue: color.r)
            .AddSlider("G", g =>
            {
                color.g = g;
                onValueChanged(color);
            }, initialValue: color.g)
            .AddSlider("B", b =>
            {
                color.b = b;
                onValueChanged(color);
            }, initialValue: color.b)
            .AddSlider("A", a =>
            {
                color.a = a;
                onValueChanged(color);
            }, initialValue: color.a);
        }
    }
}