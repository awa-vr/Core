#if VRCSDK3_AVATARS
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VRC.SDK3.Avatars.Components;

namespace AwAVR
{
    public partial class Core
    {
        /// <summary>
        /// UI Toolkit version of <see cref="Core.GetAvatar"/>: an avatar field with a refresh button and,
        /// when the scene has several avatars, one button per avatar.
        /// The avatar list refreshes itself when the hierarchy changes (avatars added, removed, enabled or disabled).
        /// Listen for changes with <c>RegisterValueChangedCallback</c>.
        /// </summary>
        public class AvatarSelector : VisualElement, INotifyValueChanged<VRCAvatarDescriptor>
        {
            public const string UssClassName = "awa-avatar-selector";

            private readonly ObjectField _field;
            private readonly VisualElement _avatarButtons;
            private List<VRCAvatarDescriptor> _avatars = new List<VRCAvatarDescriptor>();
            private VRCAvatarDescriptor _value;
            private IVisualElementScheduledItem _pendingRefresh;

            /// <summary>Preferred avatar when nothing is selected yet, e.g. the one using the inspected asset.</summary>
            public Func<VRCAvatarDescriptor, bool> FavoredAvatar { get; set; }

            /// <summary>When nothing is selected and no avatar is favored, select the first avatar in the scene.</summary>
            public bool AutoSelectFirst { get; set; }

            public IReadOnlyList<VRCAvatarDescriptor> Avatars => _avatars;

            public AvatarSelector(string label = "Avatar")
            {
                AddToClassList(UssClassName);

                var row = UI.CreateRow();
                _field = new ObjectField(label)
                {
                    objectType = typeof(VRCAvatarDescriptor),
                    allowSceneObjects = true
                };
                _field.style.flexGrow = 1;
                _field.style.flexShrink = 1;
                _field.RegisterValueChangedCallback(evt =>
                {
                    evt.StopPropagation();
                    value = evt.newValue as VRCAvatarDescriptor;
                });
                row.Add(_field);

                var refresh = new Button(Refresh) { text = "Refresh", tooltip = "Search the open scenes for avatars" };
                refresh.style.width = 80;
                row.Add(refresh);
                Add(row);

                _avatarButtons = UI.CreateRow();
                _avatarButtons.style.flexWrap = Wrap.Wrap;
                _avatarButtons.style.display = DisplayStyle.None;
                Add(_avatarButtons);

                RegisterCallback<AttachToPanelEvent>(_ => EditorApplication.hierarchyChanged += OnHierarchyChanged);
                RegisterCallback<DetachFromPanelEvent>(_ => EditorApplication.hierarchyChanged -= OnHierarchyChanged);
            }

            public VRCAvatarDescriptor value
            {
                get => _value;
                set
                {
                    if (_value == value) return;
                    var previous = _value;
                    SetValueWithoutNotify(value);
                    using (var evt = ChangeEvent<VRCAvatarDescriptor>.GetPooled(previous, value))
                    {
                        evt.target = this;
                        SendEvent(evt);
                    }
                }
            }

            public void SetValueWithoutNotify(VRCAvatarDescriptor newValue)
            {
                _value = newValue;
                _field.SetValueWithoutNotify(newValue);
                RebuildAvatarButtons();
            }

            /// <summary>
            /// Re-scans the scene. Keeps the current avatar if it still exists, otherwise picks the favored avatar,
            /// the only avatar, or (with <see cref="AutoSelectFirst"/>) the first one.
            /// </summary>
            public void Refresh()
            {
                _avatars = (GetAvatarsInScene() ?? new List<VRCAvatarDescriptor>()).Where(a => a).ToList();

                if (!_value)
                {
                    VRCAvatarDescriptor pick = null;
                    if (FavoredAvatar != null) pick = _avatars.FirstOrDefault(FavoredAvatar);
                    if (!pick && (_avatars.Count == 1 || (AutoSelectFirst && _avatars.Count > 0))) pick = _avatars[0];

                    if (pick)
                    {
                        value = pick;
                        return;
                    }

                    // The selected avatar was deleted: clear the field so it doesn't show "Missing".
                    if (!ReferenceEquals(_value, null)) value = null;
                }

                RebuildAvatarButtons();
            }

            private void OnHierarchyChanged()
            {
                // Hierarchy events come in bursts, refresh once they settle.
                _pendingRefresh?.Pause();
                _pendingRefresh = schedule.Execute(Refresh).StartingIn(100);
            }

            private void RebuildAvatarButtons()
            {
                _avatarButtons.Clear();
                var showButtons = _avatars.Count(a => a) > 1;
                _avatarButtons.style.display = showButtons ? DisplayStyle.Flex : DisplayStyle.None;
                if (!showButtons) return;

                foreach (var avatar in _avatars.Where(a => a))
                {
                    var target = avatar;
                    var button = new Button(() => value = target) { text = avatar.name };
                    button.style.flexGrow = 1;
                    if (avatar == _value)
                    {
                        button.style.unityFontStyleAndWeight = FontStyle.Bold;
                        button.style.backgroundColor = new Color(0.24f, 0.49f, 0.9f, 0.45f);
                    }

                    _avatarButtons.Add(button);
                }
            }
        }
    }
}
#endif
