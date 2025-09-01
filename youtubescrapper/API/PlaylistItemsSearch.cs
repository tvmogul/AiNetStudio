using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeScrapper
{
	public class PlaylistItemsSearch
	{
        static List<PlaylistItemsSearchComponents> items;

        private String Title;
        private String Author;
        private String Duration;
        private String Url;
        private String Thumbnail;

        public async Task<List<PlaylistItemsSearchComponents>> GetPlaylistItems(string Playlisturl)
        {
            items = new List<PlaylistItemsSearchComponents>();

            // Do search
            // Search address
            string content = await Web.getContentFromUrlWithProperty(Playlisturl);

            // Search string
            string pattern = "playlistPanelVideoRenderer\":\\{\"title\":\\{\"accessibility\":\\{\"accessibilityData\":\\{\"label\":.*?\\,\"simpleText\":\"(?<TITLE>.*?)\".*?runs\":\\[\\{\"text\":\"(?<AUTHOR>.*?)\".*?\":\\{\"thumbnails\":\\[\\{\"url\":\"(?<THUMBNAIL>.*?)\".*?\"}},\"simpleText\":\"(?<DURATION>.*?)\".*?videoId\":\"(?<URL>.*?)\"";
            MatchCollection result = Regex.Matches(content, pattern, RegexOptions.Singleline);

            for (int ctr = 0; ctr <= result.Count - 1; ctr++)
            {
                // Title
                Title = result[ctr].Groups[1].Value.Replace(@"\u0026", "&");

                // Author
                Author = result[ctr].Groups[2].Value.Replace(@"\u0026", "&");

                // Duration
                Duration = result[ctr].Groups[4].Value;

                // Thumbnail
                Thumbnail = result[ctr].Groups[3].Value;

                // Url
                Url = "http://youtube.com/watch?v=" + result[ctr].Groups[5].Value;

                // Add item to list
                items.Add(new PlaylistItemsSearchComponents(Utilities.HtmlDecode(Title),
                    Utilities.HtmlDecode(Author), Duration, Url, Thumbnail));
            }

            return items;
        }
    }
}
