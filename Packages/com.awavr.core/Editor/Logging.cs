using UnityEngine;

namespace AwAVR
{
    public partial class Core
    {
        public class Logger
        {
            private string _toolName;

            public Logger(string toolName)
            {
                _toolName = toolName;
            }

            public void LogError(string message)
            {
                Debug.LogError($"[<color=red>{_toolName}</color>] {message}");
            }

            public void Log(string message)
            {
                Debug.Log(message);
            }

            public void LogGreen(string message)
            {
                Debug.Log($"[<color=green>{_toolName}</color>] {message}");
            }

            public void LogWarning(string message)
            {
                Debug.LogWarning($"[<color=yellow>{_toolName}</color>] {message}");
            }
        }
    }
}