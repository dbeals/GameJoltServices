namespace GameJolt
{
	internal static class Constants
	{
		public static class UserUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/users/?format=json";
			public static readonly string AuthUrl = @"http://gamejolt.com/api/game/v1/users/auth/?format=dump";
		}

		public static class SessionUrls
		{
			public static readonly string OpenUrl = @"http://gamejolt.com/api/game/v1/sessions/open/?format=dump";
			public static readonly string PingUrl = @"http://gamejolt.com/api/game/v1/sessions/ping/?format=dump";
			public static readonly string CloseUrl = @"http://gamejolt.com/api/game/v1/sessions/close/?format=dump";
		}

		public static class TrophyUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/trophies/?format=json";
			public static readonly string AddAchieveUrl = @"http://gamejolt.com/api/game/v1/trophies/add-achieved/?format=dump";
		}

		public static class DataStoreUrls
		{
			public static readonly string GetKeysUrl = @"http://gamejolt.com/api/game/v1/data-store/get-keys?format=json";
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/data-store/?format=dump";
			public static readonly string SetUrl = @"http://gamejolt.com/api/game/v1/data-store/set/?format=dump";
			public static readonly string UpdateUrl = @"http://gamejolt.com/api/game/v1/data-store/update/?format=dump";
			public static readonly string RemoveUrl = @"http://gamejolt.com/api/game/v1/data-store/remove/?format=dump";
		}

		public static class ScoreUrls
		{
			public static readonly string GetUrl = @"http://gamejolt.com/api/game/v1/scores/?format=json";
			public static readonly string AddUrl = @"http://gamejolt.com/api/game/v1/scores/add/?format=dump";
			public static readonly string TablesUrl = @"http://gamejolt.com/api/game/v1/scores/tables/?format=json";
		}
	}
}