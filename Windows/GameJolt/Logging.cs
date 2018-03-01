using System;

namespace GameJolt
{
	public static class Logging
	{
		public static Action<string, string> EntryLogged { get; set; }

		public static bool IsEnabled => EntryLogged != null;

		public static bool IgnoreInformation { get; set; }

		public static bool IgnoreWarnings { get; set; }

		public static bool IgnoreErrors { get; set; }

		static Logging()
		{
#if DEBUG
			EntryLogged = (type, text) => Console.WriteLine("{0} - {1}", type, text);

			IgnoreInformation = false;
			IgnoreWarnings = false;
#else
			IgnoreInformation = true;
			IgnoreWarnings = true;
#endif
			IgnoreErrors = false;
		}

		public static void LogInformation(string text)
		{
			if (IgnoreInformation)
				return;
			LogEntry("Information", text);
		}

		public static void LogWarning(string text)
		{
			if (IgnoreWarnings)
				return;
			LogEntry("Warning", text);
		}

		public static void LogError(string text)
		{
			if (IgnoreErrors)
				return;
			LogEntry("Error", text);
		}

		private static void LogEntry(string type, string text)
		{
			EntryLogged?.Invoke(type, text);
		}
	}
}