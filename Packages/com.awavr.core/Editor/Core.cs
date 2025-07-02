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
        /// <summary>
        ///     Generates a title header for the editor window.
        /// </summary>
        /// <param name="name">Name of the window</param>
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
            EditorGUILayout.Space(20);
        }

        /// <summary>
        ///     Gets the FX controller.
        /// </summary>
        /// <param name="avatar">avatar descriptor</param>
        /// <returns>FX controller</returns>
        public static AnimatorController GetFXController(VRCAvatarDescriptor avatar)
        {
            if (avatar.baseAnimationLayers[4].animatorController != null)
                return (AnimatorController)avatar.baseAnimationLayers[4].animatorController;
            else
                return null;
        }

        /// <summary>
        ///     Gets all avatar descriptors in the scene.
        /// </summary>
        /// <returns>List of avatar descriptors</returns>
        public static List<VRCAvatarDescriptor> GetAvatarsInScene()
        {
            var avatars = SceneAsset.FindObjectsOfType<VRCAvatarDescriptor>();

            if (avatars.Length == 0)
                return null;

            return avatars.ToList();
        }

        public static void Cleany(UnityEngine.Object dirtyObject = null)
        {
            EditorUtility.SetDirty(dirtyObject);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static string SanitizeFloatInput(string input)
        {
            if (string.IsNullOrEmpty(input)) return "0";
            // Only allow digits, one leading minus, and at most one dot
            bool hasDot = false;
            bool hasMinus = false;
            var result = "";
            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (c >= '0' && c <= '9')
                {
                    result += c;
                }
                else if (c == '.' && !hasDot)
                {
                    result += c;
                    hasDot = true;
                }
                else if (c == '-' && i == 0 && !hasMinus)
                {
                    result += c;
                    hasMinus = true;
                }
                // Ignore all other characters
            }

            return result;
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
    }
}