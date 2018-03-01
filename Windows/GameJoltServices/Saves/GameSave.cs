using System;
using System.IO;

namespace GameJoltServices.Saves
{
	public sealed class GameSave
	{
		public string Username { get; set; }
		public string UserToken { get; set; }
		public string Location { get; set; }
		public string Name { get; set; }
		public DateTime DateSaved { get; set; }
		public string DescriptionData { get; set; }
		public string SaveData { get; set; }
		public bool NeedsToBeDownloaded { get; set; }
		public bool IsCached => !string.IsNullOrWhiteSpace(Location) && Path.IsPathRooted(Location);

		public Stream GetDescriptionStream()
		{
			if (!IsCached)
				throw new GameSaveNotAvailableException(this);

			if (string.IsNullOrWhiteSpace(DescriptionData))
				LoadData();
			return new MemoryStream(Convert.FromBase64String(DescriptionData));
		}

		public Stream GetDataStream()
		{
			if (!IsCached)
				throw new GameSaveNotAvailableException(this);

			if (string.IsNullOrWhiteSpace(SaveData))
				LoadData();
			return new MemoryStream(Convert.FromBase64String(SaveData));
		}

		private void LoadData()
		{
			using (var fileStream = new FileStream(Location, FileMode.Open))
			{
				using (var reader = new BinaryReader(fileStream))
				{
					var key = reader.ReadString();
					var user = reader.ReadString();
					DescriptionData = reader.ReadString();
					SaveData = reader.ReadString();
				}
			}
		}
	}
}