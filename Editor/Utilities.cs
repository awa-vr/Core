using UnityEditor;
using UnityEngine;

namespace AwAVR
{
    public partial class Core
    {
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

        public static void Cleany(Object dirtyObject = null)
        {
            EditorUtility.SetDirty(dirtyObject);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void CleanObjects(Object[] dirtyObjects = null)
        {
            foreach (var dirtyObject in dirtyObjects)
            {
                EditorUtility.SetDirty(dirtyObject);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}