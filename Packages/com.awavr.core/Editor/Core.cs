using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AwAVR
{
    public class Core
    {
        public enum ExpressionAnimatorType
        {
            Base,
            Additive,
            Gesture,
            Action,
            FX
        }

        public static void Title(string name)
        {
            EditorGUILayout.BeginVertical(GUI.skin.window, GUILayout.Height(25));

            // Title
            EditorGUILayout.LabelField(name, new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 16,
                fixedHeight = 17
            });

            // Author
            var style = new GUIStyle(EditorStyles.miniLabel)
            {
                alignment = TextAnchor.MiddleCenter,
            };
            if (GUILayout.Button("by AwA", style))
            {
                Application.OpenURL("https://linktr.ee/awa_vr");
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        public static AnimatorController GetFXController(VRCAvatarDescriptor avatar)
        {
            return GetAnimatorController(avatar);
        }

        public static AnimatorController GetAnimatorController(VRCAvatarDescriptor avatar,
            ExpressionAnimatorType type = ExpressionAnimatorType.FX)
        {
            if (avatar.baseAnimationLayers[(int)type].animatorController)
                return (AnimatorController)avatar.baseAnimationLayers[(int)type].animatorController;

            return null;
        }

        public static List<VRCAvatarDescriptor> GetAvatarsInScene()
        {
            var avatars = SceneAsset.FindObjectsOfType<VRCAvatarDescriptor>();

            if (avatars.Length == 0)
                return null;

            return avatars.ToList();
        }

        public static void Cleany(Object dirtyObject = null)
        {
            EditorUtility.SetDirty(dirtyObject);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static AnimationClip CreateNewClip(string path, string clipName)
        {
            // Create a new AnimationClip instance
            AnimationClip newClip = new AnimationClip();
            newClip.name = clipName;

            // Save as asset in the project at the specified path
            AssetDatabase.CreateAsset(newClip, System.IO.Path.Combine(path, clipName + ".anim"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newClip;
        }

        public static AnimatorControllerLayer GetLayerByName(AnimatorController controller, string name)
        {
            foreach (var layer in controller.layers)
            {
                if (layer.name == $"AwA - {name} - DO NOT TOUCH")
                    return layer;
            }

            return null;
        }

        public static void GetAvatar(ref VRCAvatarDescriptor _avatar, ref List<VRCAvatarDescriptor> _avatars)
        {
            EditorGUILayout.BeginHorizontal();
            _avatar = EditorGUILayout.ObjectField("Avatar", _avatar, typeof(VRCAvatarDescriptor),
                true) as VRCAvatarDescriptor;
            if (GUILayout.Button("Refresh", GUILayout.Width(80)))
            {
                _avatars = Core.GetAvatarsInScene();
                if (_avatars.Count == 1)
                {
                    _avatar = _avatars.First();
                    _avatars.Clear();
                }
            }

            EditorGUILayout.EndHorizontal();

            if (_avatars != null)
            {
                EditorGUILayout.BeginHorizontal();
                foreach (var avatar in _avatars)
                {
                    if (GUILayout.Button(avatar.name))
                    {
                        _avatar = avatar;
                        _avatars = null;
                    }
                }

                GUILayout.EndHorizontal();
            }
        }
    }
}