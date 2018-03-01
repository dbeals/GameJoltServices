using System;
using System.Collections.Generic;
using System.IO;
using GameJolt;

namespace GameJoltServices.News
{
	public sealed class NewsService : ServiceBase
	{
		public NewsService(ServicesManager manager)
			: base(manager) { }

		public IAsyncResult BeginGetNews()
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews");
		}

		public NewsItem[] EndGetNews(IAsyncResult result)
		{
			var dataStore = DataStore.EndGetByKey(result);
			return dataStore == null ? new NewsItem[0] : GetNewsFromData(dataStore.Data);
		}

		public NewsItem[] GetNews()
		{
			var dataStore = DataStore.GetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews");
			return dataStore == null ? new NewsItem[0] : GetNewsFromData(dataStore.Data);
		}

		public IAsyncResult GetNews(Action<NewsItem[]> callback)
		{
			return DataStore.BeginGetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews", result => callback(EndGetNews(result)));
		}

		public IAsyncResult BeginSetNews(NewsItem[] newsItems)
		{
			return DataStore.BeginSetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews", NewsToData(newsItems));
		}

		public bool EndSetNews(IAsyncResult result)
		{
			return RequestHelper.WasSuccessful(DataStore.EndSetByKey(result));
		}

		public bool SetNews(NewsItem[] newsItems)
		{
			return RequestHelper.WasSuccessful(DataStore.SetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews", NewsToData(newsItems)));
		}

		public IAsyncResult SetNews(NewsItem[] newsItems, Action<bool> callback)
		{
			return DataStore.BeginSetByKey(Manager.GameId, Manager.PrivateKey, null, null, "GameNews", NewsToData(newsItems), result => callback(EndSetNews(result)));
		}

		private static NewsItem[] GetNewsFromData(string data)
		{
			var output = new List<NewsItem>();
			using (var stream = new MemoryStream(Convert.FromBase64String(data)))
			{
				using (var reader = new BinaryReader(stream))
				{
					while (stream.Position < stream.Length)
					{
						output.Add(new NewsItem
						{
							Title = reader.ReadString(),
							Content = reader.ReadString(),
							DateTimePosted = new DateTime(reader.ReadInt64())
						});
					}
				}
			}

			return output.ToArray();
		}

		private static string NewsToData(IEnumerable<NewsItem> newsItems)
		{
			using (var stream = new MemoryStream())
			{
				using (var writer = new BinaryWriter(stream))
				{
					foreach (var item in newsItems)
					{
						writer.Write(item.Title);
						writer.Write(item.Content);
						writer.Write(item.DateTimePosted.Ticks);
					}

					return Convert.ToBase64String(stream.ToArray());
				}
			}
		}
	}
}