using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using GameJolt;
using GameJoltServices;
using GameJoltServices.News;
using GameJoltServices.Saves;
using User = GameJoltServices.User;

namespace TestBed
{
	internal class Program
	{
		#region Methods
		private static void WaitForAsyncOperation(IAsyncResult result, string message)
		{
			var lineIndex = Console.CursorTop;
			var dotCount = 0;
			var timer = new Timer(250);
			timer.Elapsed += (sender, args) =>
			{
				++dotCount;
				if (dotCount > 3)
					dotCount = 0;
			};

			timer.Start();
			while (!result.IsCompleted)
			{
				Console.CursorTop = lineIndex;
				Console.CursorLeft = 0;
				Console.Write(message + new string('.', dotCount) + new string(' ', 3 - dotCount));
			}

			Console.CursorTop = lineIndex;
			Console.CursorLeft = 0;
			Console.WriteLine("Finished " + message);
		}

		private static T WaitForAsyncOperation<T>(Task<T> task, string message)
		{
			var lineIndex = Console.CursorTop;
			var dotCount = 0;
			var timer = new Timer(250);
			timer.Elapsed += (sender, args) =>
			{
				++dotCount;
				if (dotCount > 3)
					dotCount = 0;
			};

			timer.Start();
			while (!task.IsCompleted)
			{
				Console.CursorTop = lineIndex;
				Console.CursorLeft = 0;
				Console.Write(message + new string('.', dotCount) + new string(' ', 3 - dotCount));
			}

			Console.CursorTop = lineIndex;
			Console.CursorLeft = 0;
			Console.WriteLine("Finished " + message);
			return task.Result;
		}

		private static void WriteUser(User user)
		{
			Console.WriteLine($"User ID: {user.UserId}");
			Console.WriteLine($"Username: {user.Username}");
			Console.WriteLine($"Last Logged In: {user.LastLoggedIn}");
			Console.WriteLine($"Avatar URL: {user.AvatarUrl}");
			Console.WriteLine($"Developer Name: {user.DeveloperName}");
			Console.WriteLine($"Develoer Website: {user.DeveloperWebsite}");
			Console.WriteLine($"Developer Description: {user.DeveloperDescription}");
		}

		private static void WriteTrophy(Trophy trophy)
		{
			Console.WriteLine($"({trophy.Id}) {trophy.Title} - \"{trophy.Description}\" - {(bool.Parse(trophy.Achieved) ? "Achieved" : "Locked")}");
		}

		private static void Main(string[] args)
		{
			const string gameId = "YOURGAMEID";
			const string privateKey = "YOURGAMEKEY";
			if (string.Compare(gameId, "YOURGAMEID", StringComparison.OrdinalIgnoreCase) == 0)
				throw new InvalidOperationException("You have to set your game id first.");
			if (string.Compare(privateKey, "YOURGAMEKEY", StringComparison.OrdinalIgnoreCase) == 0)
				throw new InvalidOperationException("You have to set your private key first.");

			var servicesManager = new ServicesManager(gameId, privateKey);
			var gameSaveService = new GameSaveService(ServicesManager.GetDefaultCacheFolder("GJS Test Bed"), servicesManager);

			Console.Write("Username: ");
			var username = Console.ReadLine();
			Console.Write("Token: ");
			var userToken = Console.ReadLine();

			var user = servicesManager.LogInUser(username, userToken);
			if (user == null)
				Console.WriteLine("Failed to authenticate " + username);
			else
			{
				WriteUser(user);

				var newSave = new GameSave
				{
					Location = "YMMGSave1",
					Name = "Donny 2",
					DateSaved = DateTime.Now,
					Username = user.Username,
					UserToken = user.UserToken
				};

				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						writer.Write("Tungala City Square");
						writer.Write("Donny");
						writer.Write(new TimeSpan(6, 11, 15).Ticks);

						newSave.DescriptionData = Convert.ToBase64String(stream.ToArray());
					}
				}

				using (var stream = new MemoryStream())
				{
					using (var writer = new BinaryWriter(stream))
					{
						writer.Write("tungala_02");
						writer.Write("Donny");
						writer.Write("Warrior");
						writer.Write("97");

						// other data here

						newSave.SaveData = Convert.ToBase64String(stream.ToArray());
					}
				}

				var uploadResult = gameSaveService.UploadSave(newSave);

				var newsService = new NewsService(servicesManager);
				var newsItems = newsService.GetNews();
				if (newsItems.Length == 0)
					Console.WriteLine("No news items.");
				else
				{
					Console.WriteLine("News: ");
					foreach (var newsItem in newsItems)
						Console.WriteLine($"{newsItem.DateTimePosted.ToShortDateString()} - {newsItem.Title}\n{newsItem.Content}\n");
				}

				var saves = gameSaveService.GetSaves(user);
				if (saves.Length == 0)
					Console.WriteLine("No game saves.");
				else
				{
					foreach (var save in saves)
					{
						if (!save.IsCached)
							gameSaveService.DownloadSave(save);

						Console.WriteLine("{0} - {1} - {2}", save.Location, save.Name, save.DateSaved);
						using (var stream = save.GetDataStream())
						{
							using (var reader = new BinaryReader(stream))
							{
								Console.WriteLine(reader.ReadString());
								Console.WriteLine(reader.ReadString());
								Console.WriteLine(reader.ReadString());
								Console.WriteLine(reader.ReadString());
							}
						}
					}
				}
			}

			Console.ReadKey(true);
		}
		#endregion
	}
}