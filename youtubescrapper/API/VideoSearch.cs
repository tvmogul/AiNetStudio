using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace YouTubeScrapper
{
    public class VideoSearch
    {
        static List<VideoSearchComponents> items;

        static string title;
        static string author;
        static string description;
        static string duration;
        static string url;
        static string thumbnail;
        static string viewcount;

        //string content = await Web.getContentFromUrl("https://www.youtube.com/watch?v=" + videoid);

        public async Task<List<VideoSearchComponents>> GetVideo(string querystring, int querypages)
        {
            items = new List<VideoSearchComponents>();

            // Do search
            for (int i = 1; i <= querypages; i++)
            {
                // Search address
                string content = await Web.getContentFromUrl("https://www.youtube.com/embed/" + querystring + "&page=" + i);

                content = Helper.ExtractValue(content, "ytInitialData", "ytInitialPlayerResponse");

                // Search string
                string pattern = "videoRenderer.*?serviceEndpoint";
                MatchCollection result = Regex.Matches(content, pattern, RegexOptions.Singleline);

                for (int ctr = 0; ctr <= result.Count - 1; ctr++)
                {
                    // Title
                    title = Helper.ExtractValue(result[ctr].Value, "\"title\":{\"runs\":[{\"text\":\"", "\"}]").Replace(@"\u0026", "&");

                    // Author
                    author = Helper.ExtractValue(result[ctr].Value, "\"ownerText\":{\"runs\":[{\"text\":\"", "\",\"").Replace(@"\u0026", "&");

                    // Description
                    description = Helper.ExtractValue(result[ctr].Value, "descriptionSnippet\":{\"runs\":[{\"text\":\"", "\"}]},").Replace(@"\u0026", "&");

                    // Duration
                    duration = Helper.ExtractValue(result[ctr].Value, "lengthText\"", "viewCountText");
                    duration = Helper.ExtractValue(duration, "simpleText\":\"", "\"");

                    // Url
                    url = string.Concat("http://www.youtube.com/watch?v=", Helper.ExtractValue(result[ctr].Value, "videoId\":\"", "\""));

                    // Thumbnail
                    thumbnail = Helper.ExtractValue(result[ctr].Value, "\"thumbnail\":{\"thumbnails\":[{\"url\":\"", "\"").Replace(@"\u0026", "&");

                    // View count
                    {
                        string strView = Helper.ExtractValue(result[ctr].Value, "\"viewCountText\":{\"simpleText\":\"", "\"},\"");
                        if (strView.IsValid())//if (!string.IsNullOrEmpty(strView) && !string.IsNullOrWhiteSpace(strView))
                        {
                            string[] strParsedArr =
                                strView.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            string parsedText = strParsedArr[0];
                            parsedText = parsedText.Trim().Replace(",", ".");

                            viewcount = parsedText;
                        }
                    }

                    // Remove playlists
                    if (title != "__title__" && title.IsValid()/*title != " "*/)
                    {
                        if (duration.IsValid())//if (duration != "" && duration != " ")
                        {
                            // Add item to list
                            items.Add(new VideoSearchComponents(Utilities.HtmlDecode(title),
                                Utilities.HtmlDecode(author), Utilities.HtmlDecode(description), duration, url, thumbnail, viewcount));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Search videos
        /// </summary>
        /// <param name="querystring"></param>
        /// <param name="querypages"></param>
        /// <returns></returns>
        public async Task<List<VideoSearchComponents>> GetVideos(string querystring, int querypages)
        {
            items = new List<VideoSearchComponents>();

            // Do search
            for (int i = 1; i <= querypages; i++)
            {
                // Search address
                string content = await Web.getContentFromUrl("https://www.youtube.com/results?search_query=" + querystring + "&page=" + i);

                content = Helper.ExtractValue(content, "ytInitialData", "ytInitialPlayerResponse");

                // Search string
                string pattern = "videoRenderer.*?serviceEndpoint";
                MatchCollection result = Regex.Matches(content, pattern, RegexOptions.Singleline);

                for (int ctr = 0; ctr <= result.Count - 1; ctr++)
                {
                    // Title
                    title = Helper.ExtractValue(result[ctr].Value, "\"title\":{\"runs\":[{\"text\":\"", "\"}]").Replace(@"\u0026", "&");

                    // Author
                    author = Helper.ExtractValue(result[ctr].Value, "\"ownerText\":{\"runs\":[{\"text\":\"", "\",\"").Replace(@"\u0026", "&");

                    // Description
                    description = Helper.ExtractValue(result[ctr].Value, "descriptionSnippet\":{\"runs\":[{\"text\":\"", "\"}]},").Replace(@"\u0026", "&");

                    // Duration
                    duration = Helper.ExtractValue(result[ctr].Value, "lengthText\"", "viewCountText");
                    duration = Helper.ExtractValue(duration, "simpleText\":\"", "\"");

                    // Url
                    url = string.Concat("http://www.youtube.com/watch?v=", Helper.ExtractValue(result[ctr].Value, "videoId\":\"", "\""));

                    // Thumbnail
                    thumbnail = Helper.ExtractValue(result[ctr].Value, "\"thumbnail\":{\"thumbnails\":[{\"url\":\"", "\"").Replace(@"\u0026", "&");

                    // View count
                    {
                        string strView = Helper.ExtractValue(result[ctr].Value, "\"viewCountText\":{\"simpleText\":\"", "\"},\"");
                        if (strView.IsValid())//if (!string.IsNullOrEmpty(strView) && !string.IsNullOrWhiteSpace(strView))
                        {
                            string[] strParsedArr =
                                strView.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                            string parsedText = strParsedArr[0];
                            parsedText = parsedText.Trim().Replace(",", ".");

                            viewcount = parsedText;
                        }
                    }

                    // Remove playlists
                    if (title != "__title__" && title.IsValid()/*title != " "*/)
                    {
                        if (duration.IsValid())//if (duration != "" && duration != " ")
                        {
                            // Add item to list
                            items.Add(new VideoSearchComponents(Utilities.HtmlDecode(title), 
                                Utilities.HtmlDecode(author), Utilities.HtmlDecode(description), duration, url, thumbnail, viewcount));
                        }
                    }
                }
            }

            return items;
        }

        /// <summary>
        /// Search videos paged
        /// </summary>
        /// <param name="querystring"></param>
        /// <param name="querypage"></param>
        /// <returns></returns>
        public async Task<List<VideoSearchComponents>> GetVideosPaged(string querystring, int querypagenum)
        {
            items = new List<VideoSearchComponents>();

            // Do search
            // Search address
            string content = await Web.getContentFromUrl("https://www.youtube.com/results?search_query=" + querystring + "&page=" + querypagenum);

            content = Helper.ExtractValue(content, "ytInitialData", "ytInitialPlayerResponse");

            // Search string
            string pattern = "videoRenderer.*?serviceEndpoint";
            MatchCollection result = Regex.Matches(content, pattern, RegexOptions.Singleline);

            for (int ctr = 0; ctr <= result.Count - 1; ctr++)
            {
                // Title
                title = Helper.ExtractValue(result[ctr].Value, "\"title\":{\"runs\":[{\"text\":\"", "\"}]").Replace(@"\u0026", "&");

                // Author
                author = Helper.ExtractValue(result[ctr].Value, "\"ownerText\":{\"runs\":[{\"text\":\"", "\",\"").Replace(@"\u0026", "&");

                // Description
                description = Helper.ExtractValue(result[ctr].Value, "descriptionSnippet\":{\"runs\":[{\"text\":\"", "\"}]},").Replace(@"\u0026", "&");

                // Duration
                duration = Helper.ExtractValue(result[ctr].Value, "lengthText\"", "viewCountText");
                duration = Helper.ExtractValue(duration, "simpleText\":\"", "\"");

                // Url
                url = string.Concat("http://www.youtube.com/watch?v=", Helper.ExtractValue(result[ctr].Value, "videoId\":\"", "\""));

                // Thumbnail
                thumbnail = Helper.ExtractValue(result[ctr].Value, "\"thumbnail\":{\"thumbnails\":[{\"url\":\"", "\"").Replace(@"\u0026", "&");

                // View count
                {
                    string strView = Helper.ExtractValue(result[ctr].Value, "\"viewCountText\":{\"simpleText\":\"", "\"},\"");
                    if(strView.IsValid())//if (!string.IsNullOrEmpty(strView) && !string.IsNullOrWhiteSpace(strView))
                    {
                        string[] strParsedArr =
                            strView.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                        string parsedText = strParsedArr[0];
                        parsedText = parsedText.Trim().Replace(",", ".");

                        viewcount = parsedText;
                    }
                }

                // Remove playlists
                if (title != "__title__" && title.IsValid() /*!= " "*/)
                {
                    if  (duration.IsValid())//(!string.IsNullOrEmpty(duration) && duration != " ") // The second condition can be !string.IsNullOrWhiteSpace(duration).
                    {
                        // Add item to list
                        items.Add(new VideoSearchComponents(Utilities.HtmlDecode(title),
                            Utilities.HtmlDecode(author), Utilities.HtmlDecode(description), duration, url, thumbnail, viewcount));
                    }
                }
            }

            return items;
        }
    }
}
