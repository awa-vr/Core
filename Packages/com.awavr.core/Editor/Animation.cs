using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

namespace AwAVR
{
    public partial class Core
    {
        public enum ExpressionAnimatorType
        {
            Base,
            Additive,
            Gesture,
            Action,
            FX
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
    }
}