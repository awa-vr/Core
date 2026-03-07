#if VRCSDK3_AVATARS
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AwAVR
{
    public partial class Core
    {
        public static void GetAvatar(ref VRCAvatarDescriptor _avatar, ref List<VRCAvatarDescriptor> _avatars)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                _avatar = EditorGUILayout.ObjectField("Avatar", _avatar, typeof(VRCAvatarDescriptor),
                    true) as VRCAvatarDescriptor;
                if (GUILayout.Button("Refresh", GUILayout.Width(80)))
                {
                    RefreshAvatars(ref _avatar, ref _avatars);
                }
            }

            DrawMultipleAvatarSelection(ref _avatar, ref _avatars);
        }

        public static List<VRCAvatarDescriptor> GetAvatarsInScene()
        {
            var avatars = SceneAsset.FindObjectsOfType<VRCAvatarDescriptor>();

            if (avatars.Length == 0)
                return null;

            return avatars.ToList();
        }

        private static void RefreshAvatars(ref VRCAvatarDescriptor _avatar, ref List<VRCAvatarDescriptor> _avatars)
        {
            _avatars = GetAvatarsInScene();

            if (_avatars == null)
                return;

            if (_avatars.Count == 1)
            {
                _avatar = _avatars.First();
                _avatars.Clear();
            }
        }

        private static void DrawMultipleAvatarSelection(ref VRCAvatarDescriptor _avatar,
            ref List<VRCAvatarDescriptor> _avatars)
        {
            if (_avatars != null && _avatars.Count > 1)
            {
                using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    foreach (var avatar in _avatars)
                    {
                        if (GUILayout.Button(avatar.name))
                        {
                            _avatar = avatar;
                        }
                    }
                }
            }
        }
    }
}
#endif