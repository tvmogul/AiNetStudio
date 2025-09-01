using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeScrapper
{
	public class ChannelItemsSearch
	{
		public async Task<List<PlaylistItemsSearchComponents>> GetChannelItems(string Channelurl)
		{
			// Do search
			// Search address
			string content = await Web.getContentFromUrlWithProperty(Channelurl + "/videos");

			string Playlisturl = string.Empty;

			// Search string
			string pattern = "playAllButton.*?\"commandMetadata\":\\{\"webCommandMetadata\":\\{\"url\":\"(?<URL>.*?)\"";
			MatchCollection result = Regex.Matches(content, pattern, RegexOptions.Singleline);

			if (result.Count > 0)
				Playlisturl = "http://youtube.com" + result[0].Groups[1].Value.Replace(@"\u0026", "&");
			else
				Playlisturl = string.Empty;

			return await new PlaylistItemsSearch().GetPlaylistItems(Playlisturl);
		}
	}
}
