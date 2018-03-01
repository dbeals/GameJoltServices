using System.IO;
using Android.App;
using Android.OS;
using Android.Widget;
using GameJoltServices;
using GameJoltServices.News;
using GameJoltServices.Saves;

namespace TestApp
{
	[Activity(Label = "TestApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class Activity1 : Activity
	{
		#region Methods
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			var button = FindViewById<Button>(Resource.Id.MyButton);
			var textView = FindViewById<TextView>(Resource.Id.textView1);

			button.Click += delegate
			{
				const string gameId = "GAMEID";
				const string privateKey = "PRIVATEKEY";
				var servicesManager = new ServicesManager(gameId, privateKey);

				const string username = "USERNAME";
				const string userToken = "USERTOKEN";
				button.Enabled = false;

				textView.Text = string.Empty;
				var user = servicesManager.LogInUser(username, userToken);
				if (user == null)
					textView.Text += ("Failed to authenticate " + username) + "\n";
				else
				{
					var newsService = new NewsService(servicesManager);
					var newsItems = newsService.GetNews();
					if (newsItems.Length == 0)
						textView.Text += ("No news items.") + "\n";
					else
					{
						textView.Text += ("News: ") + "\n";
						foreach (var newsItem in newsItems)
							textView.Text += (string.Format("{0} - {1}\n{2}\n", newsItem.DateTimePosted.ToShortDateString(), newsItem.Title, newsItem.Content)) + "\n";
					}

					var gameSaveService = new GameSaveService(Helper.GetDefaultCacheFolder("GJS Test Bed"), servicesManager);
					var saves = gameSaveService.GetSaves(user);
					if (saves.Length == 0)
						textView.Text += ("No game saves.") + "\n";
					else
					{
						foreach (var save in saves)
						{
							if (!save.IsCached)
								gameSaveService.DownloadSave(save);

							textView.Text += string.Format("{0} - {1} - {2}", save.Location, save.Name, save.DateSaved) + "\n";
							using (var stream = save.GetDataStream())
							{
								using (var reader = new BinaryReader(stream))
								{
									textView.Text += (reader.ReadString()) + "\n";
									textView.Text += (reader.ReadString()) + "\n";
									textView.Text += (reader.ReadString()) + "\n";
									textView.Text += (reader.ReadString()) + "\n";
								}
							}
						}
					}
				}
			};
		}
		#endregion
	}
}