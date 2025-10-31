using ContractsInterfaces;
using UnityEngine;

namespace Infrastructure
{
    public class UnityLogger : ContractsInterfaces.ILogger
    {
        public void Debug(string message)
        {
            UnityEngine.Debug.Log($"[DEBUG] {message}");
        }

        public void Information(string message)
        {
            UnityEngine.Debug.Log($"[INFO] {message}");
        }

        public void Warning(string message)
        {
            UnityEngine.Debug.LogWarning($"[WARN] {message}");
        }

        public void Error(string message)
        {
            UnityEngine.Debug.LogError($"[ERROR] {message}");
        }
    }
}
