using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;

namespace IgniteModule.UI
{
    public class IgniteImage : IgniteGUIElement
    {
        [SerializeField] Image image;
        [SerializeField] Image background;

        float height;

        /// <summary> Image </summary>
        public Image Image { get { return image; } }
        /// <summary> Background </summary>
        public Image Background { get { return background; } }

        /// <summary> サイズ設定 </summary>
        public override void SetSize(IIgniteGUISize size)
        {
            SetSize(size.ImageHeight);
        }

        /// <summary> サイズ設定 </summary> 
        public void SetSize(float? height = null)
        {
            if (height.HasValue) this.height = height.Value;
        }

        /// <summary> テーマ設定 </summary>
        public override void SetTheme(IIgniteGUITheme theme)
        {
            SetTheme(theme.ImageBackground);
        }

        /// <summary> テーマ設定 </summary> 
        public void SetTheme(Color? backgroundColor = null)
        {
            if (backgroundColor.HasValue) background.color = backgroundColor.Value;
        }

        /// <summary> ImageのサイズをWindowの範囲内で元画像に合わせる </summary>
        public void Fit()
        {
            image.rectTransform.localScale = Vector3.one;
            image.SetNativeSize();
            if (image.rectTransform.sizeDelta.y > height)
            {
                if (image.rectTransform.sizeDelta.x != 0f)
                {
                    var scale = height / image.rectTransform.sizeDelta.y;
                    image.rectTransform.localScale = new Vector3(scale, scale, scale);
                    RectTransform.SetSizeDelta(y: image.rectTransform.sizeDelta.y * scale);
                }
            }
            background.rectTransform.sizeDelta = image.rectTransform.sizeDelta;
            background.rectTransform.localScale = image.rectTransform.localScale;
        }

        /// <summary> 選択時 </summary>
        public override IObservable<Unit> OnSelected()
        {
            return image.OnPointerClickAsObservable().AsUnitObservable();
        }

        /// <summary> 生成 </summary>
        public static IgniteImage Create(Sprite sprite, IObservable<Sprite> observableSprite = null, Action<IgniteImage> onInitialize = null, string id = "")
        {
            // 生成
            var instance = Instantiate(Resources.Load<GameObject>("IgniteGUI/Prefab/Image")).GetComponent<IgniteImage>();

            instance.ID = id;  // ID設定

            // 初期化処理
            instance.OnInitializeAsync()
            .SubscribeWithState2(instance, sprite, (_, i, s) =>
            {
                i.Image.sprite = s;
                i.Fit();
                // 親Windowのサイズが変更されたらImageサイズをそれに合わせる
                i.Window.OnSizeChanged().SubscribeWithState(instance, (__, image) => image.Fit());
            });

            // Sprite変更イベント
            if (observableSprite != null)
            {
                instance.OnInitializeAsync()
                .SubscribeWithState2(instance, observableSprite, (_, i, os) =>
                {
                    os.SubscribeWithState(i.image, (s, image) => image.sprite = s).AddTo(i);
                    os.SubscribeWithState(i, (s, image) => i.Fit()).AddTo(i);
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

        public static IIgniteGUIGroup AddImage(this IIgniteGUIGroup group, Sprite sprite, IObservable<Sprite> observableSprite = null, Action<IgniteImage> onInitialize = null, string id = "")
        {
            return group.Add(IgniteImage.Create(sprite, observableSprite, onInitialize, id));
        }
    }
}