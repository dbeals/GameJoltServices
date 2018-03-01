using System;

namespace GameJoltServices.News
{
	public sealed class NewsItem
	{
		public string Title { get; set; }
		public string Content { get; set; }
		public DateTime DateTimePosted { get; set; }
	}
}