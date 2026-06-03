//  DebugLogger


namespace Leguar.TotalJSON.Internal {

	internal static class DebugLogger {

		internal static void LogUserWarning(string str) {
			UnityEngine.Debug.LogWarning("TotalJSON: " + str);
		}

		internal static void LogInternalError(string str) {
			UnityEngine.Debug.LogError("Leguar.TotalJSON: Internal error: " + str);
		}

	}

}
