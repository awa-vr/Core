using UnityEngine;
using UnityEngine.UIElements;

namespace AwAVR
{
    public partial class Core
    {
        /// <summary>
        /// UI Toolkit building blocks shared by AwA's tools.
        /// The IMGUI helpers (<see cref="Title"/>, <see cref="GetAvatar"/>) are kept for tools that have not migrated yet.
        /// </summary>
        public static class UI
        {
            public const string AuthorUrl = "https://linktr.ee/awa_vr";

            /// <summary>UI Toolkit version of <see cref="Core.Title"/>: the tool name with a "by AwA" link under it.</summary>
            public static VisualElement CreateTitle(string name)
            {
                var container = new VisualElement { name = "awa-title" };
                container.style.alignItems = Align.Center;
                container.style.paddingTop = 6;
                container.style.paddingBottom = 6;
                container.style.marginBottom = 6;
                SetBorder(container, 1, new Color(0, 0, 0, 0.35f), 4);
                container.style.backgroundColor = new Color(0, 0, 0, 0.12f);

                var title = new Label(name);
                title.style.fontSize = 16;
                title.style.unityFontStyleAndWeight = FontStyle.Bold;
                title.style.unityTextAlign = TextAnchor.MiddleCenter;
                container.Add(title);

                var author = new Label("by AwA") { tooltip = AuthorUrl };
                author.style.fontSize = 10;
                author.style.opacity = 0.7f;
                author.style.unityTextAlign = TextAnchor.MiddleCenter;
                author.RegisterCallback<ClickEvent>(_ => Application.OpenURL(AuthorUrl));
                author.RegisterCallback<MouseEnterEvent>(_ => author.style.opacity = 1f);
                author.RegisterCallback<MouseLeaveEvent>(_ => author.style.opacity = 0.7f);
                container.Add(author);

                return container;
            }

            /// <summary>A box with the same look as IMGUI's "helpbox" style, for grouping fields.</summary>
            public static VisualElement CreateBox(string title = null)
            {
                var box = new VisualElement();
                box.AddToClassList("awa-box");
                box.style.paddingLeft = 4;
                box.style.paddingRight = 4;
                box.style.paddingTop = 3;
                box.style.paddingBottom = 3;
                box.style.marginTop = 2;
                box.style.marginBottom = 2;
                box.style.backgroundColor = new Color(0, 0, 0, 0.08f);
                SetBorder(box, 1, new Color(0, 0, 0, 0.3f), 3);

                if (!string.IsNullOrEmpty(title))
                {
                    var label = new Label(title);
                    label.style.unityTextAlign = TextAnchor.MiddleCenter;
                    label.style.unityFontStyleAndWeight = FontStyle.Bold;
                    label.style.marginBottom = 2;
                    box.Add(label);
                }

                return box;
            }

            /// <summary>A horizontal row that lays its children out left to right.</summary>
            public static VisualElement CreateRow()
            {
                var row = new VisualElement();
                row.style.flexDirection = FlexDirection.Row;
                row.style.alignItems = Align.Center;
                return row;
            }

            public static void SetBorder(VisualElement element, float width, Color color, float radius = 0)
            {
                element.style.borderLeftWidth = width;
                element.style.borderRightWidth = width;
                element.style.borderTopWidth = width;
                element.style.borderBottomWidth = width;
                element.style.borderLeftColor = color;
                element.style.borderRightColor = color;
                element.style.borderTopColor = color;
                element.style.borderBottomColor = color;
                element.style.borderTopLeftRadius = radius;
                element.style.borderTopRightRadius = radius;
                element.style.borderBottomLeftRadius = radius;
                element.style.borderBottomRightRadius = radius;
            }
        }
    }
}
