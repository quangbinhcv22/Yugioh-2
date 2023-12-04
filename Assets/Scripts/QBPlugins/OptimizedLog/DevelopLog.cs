using UnityEngine;
using Object = UnityEngine.Object;

namespace QBPlugins.OptimizedLog
{
    public static class DevelopLog
    {
        public static void Log(object message)
        {
#if UNITY_EDITOR || DEVELOPMENT_VERSION
            Debug.Log(message);
#endif
        }

        public static void Log(object message, Object context)
        {
#if UNITY_EDITOR || DEVELOPMENT_VERSION
            Debug.Log(message, context);
#endif
        }
    }
}