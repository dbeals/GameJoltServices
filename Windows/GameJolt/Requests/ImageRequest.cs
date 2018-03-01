using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GameJolt.Requests
{
	internal sealed class ImageRequest : WebRequestBase<Stream>
	{
		public ImageRequest(string url)
			: base(null, null)
		{
			UrlBase = url;
		}

		public override Stream Process(IAsyncResult result)
		{
			base.Process(result);
			var request = (HttpWebRequest) WebRequest.Create(UrlBase);
			using (var response = request.GetResponse())
			{
				using (var reader = new BinaryReader(response.GetResponseStream()))
				{
					var output = new List<byte>();

					byte[] buffer;
					while ((buffer = reader.ReadBytes(1024)).Length > 0)
						output.AddRange(buffer);

					return new MemoryStream(output.ToArray());
				}
			}
		}
	}
}