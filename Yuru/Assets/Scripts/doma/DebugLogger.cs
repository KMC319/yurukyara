using System.Diagnostics;

namespace doma{
	public static class DebugLogger{
		[Conditional("UNITY_EDITOR")]
		public static void Log(object o)
		{
			UnityEngine.Debug.Log(o);
		}
		[Conditional("UNITY_EDITOR")]
		public static void LogError(object o)
		{
			UnityEngine.Debug.unityLogger.LogError("Error",o);
		}
	}
}