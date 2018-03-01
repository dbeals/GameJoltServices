using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using BealsSoftware.Core;
using GameJolt;
using GameJoltServices;
using GameJoltServices.Saves;
using GameJoltServices.News;
using System.IO;

namespace TestApp
{
	[Activity(Label = "TestApp", MainLauncher = true, Icon = "@drawable/icon")]
	public class Activity1 : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			Logging.EntryLogged = (type, text) =>
			{
				if(type == "Information")
					RuntimeLog.LogInformation(text, "GameJolt");
				else if(type == "Warning")
					RuntimeLog.LogWarning(text, "GameJolt");
				else if(type == "Error")
					RuntimeLog.LogError(text, "GameJolt");
			};

			base.OnCreate(bundle);

			SetContentView(Resource.Layout.Main);

			var button = FindViewById<Button>(Resource.Id.MyButton);
			var textView = FindViewById<TextView>(Resource.Id.textView1);

			button.Click += delegate
			{
				string gameID = "GAMEID";
				string privateKey = "PRIVATEKEY";
				ServicesManager servicesManager = new ServicesManager(gameID, privateKey);

				var username = "USERNAME";
				var userToken = "USERTOKEN";
				button.Enabled = false;

				textView.Text = string.Empty;
				var user = servicesManager.LogInUser(username, userToken);
				if(user == null)
					textView.Text += ("Failed to authenticate " + username) + "\n";
				else
				{
					//WriteUser(user);

					NewsService newsService = new NewsService(servicesManager);
					var newsItems = newsService.GetNews();
					if(newsItems.Length == 0)
						textView.Text += ("No news items.") + "\n";
					else
					{
						textView.Text += ("News: ") + "\n";
						foreach(var newsItem in newsItems)
							textView.Text += (string.Format("{0} - {1}\n{2}\n", newsItem.DateTimePosted.ToShortDateString(), newsItem.Title, newsItem.Content)) + "\n";
					}

					var gameSaveService = new GameSaveService(GameJoltServices.Helper.GetDefaultCacheFolder("GJS Test Bed"), servicesManager);
					var saves = gameSaveService.GetSaves(user);
					if(saves.Length == 0)
						textView.Text += ("No game saves.") + "\n";
					else
					{
						foreach(var save in saves)
						{
							if(!save.IsCached)
								gameSaveService.DownloadSave(save);

							textView.Text += string.Format("{0} - {1} - {2}", save.Location, save.Name, save.DateSaved) + "\n";
							using(var stream = save.GetDataStream())
							{
								using(var reader = new BinaryReader(stream))
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
	}
}

