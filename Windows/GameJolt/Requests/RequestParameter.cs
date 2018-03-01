using System;

namespace GameJolt.Requests
{
	internal sealed class RequestParameter
	{
		internal static readonly string GameIdKey = "game_id";
		internal static readonly string UsernameKey = "username";
		internal static readonly string UserTokenKey = "user_token";
		internal static readonly string UserIdKey = "user_id";
		internal static readonly string TrophyIdKey = "trophy_id";
		internal static readonly string StatusKey = "status";
		internal static readonly string KeyKey = "key";
		internal static readonly string DataKey = "data";
		internal static readonly string OperationKey = "operation";
		internal static readonly string ValueKey = "value";
		internal static readonly string ScoreKey = "score";
		internal static readonly string SortKey = "sort";
		internal static readonly string GuestKey = "guest";
		internal static readonly string ExtraDataKey = "extra_data";
		internal static readonly string TableIdKey = "table_id";
		internal static readonly string LimitKey = "limit";

		public string Name { get; private set; }

		public object Value { get; internal set; }

		public RequestParameter(string name, object value)
		{
			Name = name;
			Value = value;
		}

		public override string ToString()
		{
			return $"{Name}={Value?.ToString() ?? string.Empty}";
		}

		public static RequestParameter CreateGameId(string value)
		{
			return new RequestParameter(GameIdKey, value);
		}

		public static RequestParameter CreateUsername(string value)
		{
			return new RequestParameter(UsernameKey, value);
		}

		public static RequestParameter CreateUserToken(string value)
		{
			return new RequestParameter(UserTokenKey, value);
		}

		public static RequestParameter CreateUserId(params string[] value)
		{
			return new RequestParameter(UserIdKey, string.Join(",", value));
		}

		public static RequestParameter CreateTrophyId(params string[] value)
		{
			return new RequestParameter(TrophyIdKey, string.Join(",", value));
		}

		public static RequestParameter CreateStatus(SessionStatus value)
		{
			return new RequestParameter(StatusKey, value.ToString().ToLower());
		}

		public static RequestParameter CreateKey(string value)
		{
			return new RequestParameter(KeyKey, value);
		}

		public static RequestParameter CreateData(string value)
		{
			return new RequestParameter(DataKey, value);
		}

		public static RequestParameter CreateOperation(DataStoreOperation value)
		{
			return new RequestParameter(OperationKey, value.ToString().ToLower());
		}

		public static RequestParameter CreateValue(object value)
		{
			return new RequestParameter(ValueKey, value);
		}

		public static RequestParameter CreateScore(object value)
		{
			return new RequestParameter(ScoreKey, value);
		}

		public static RequestParameter CreateSort(object value)
		{
			return new RequestParameter(SortKey, value);
		}

		public static RequestParameter CreateGuest(string value)
		{
			return new RequestParameter(GuestKey, value);
		}

		public static RequestParameter CreateExtraData(string value)
		{
			return new RequestParameter(ExtraDataKey, value);
		}

		public static RequestParameter CreateTableId(string value)
		{
			return new RequestParameter(TableIdKey, value);
		}

		public static RequestParameter CreateLimit(int value)
		{
			if (value < 0 || value > 100)
				throw new ArgumentOutOfRangeException($"{value} is out of the range for a limit parameter (1-100.)");
			return new RequestParameter(LimitKey, value);
		}
	}
}