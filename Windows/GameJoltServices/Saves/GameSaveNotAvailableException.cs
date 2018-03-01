using System;

namespace GameJoltServices.Saves
{
	public sealed class GameSaveNotAvailableException : Exception
	{
		public GameSaveNotAvailableException(GameSave gameSave)
			: this(gameSave, null) { }

		public GameSaveNotAvailableException(GameSave gameSave, Exception innerException)
			: base($"The game save '{gameSave.Location}' is not available locally.", innerException) { }
	}
}