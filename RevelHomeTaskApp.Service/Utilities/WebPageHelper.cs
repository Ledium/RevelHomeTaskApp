using HtmlAgilityPack;
using System.Net;
using System;

namespace RevelHomeTaskApp.Service.Utilities
{
    public static class WebPageHelper
    {
        public static HtmlDocument LoadPage(string url)
        {
            url = ValidateUrl(url);

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            var content = new HtmlDocument();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var receiveStream = response.GetResponseStream();

                content.Load(receiveStream);

                response.Close();
            } else
            {
                Console.WriteLine("Failed to load resource from provided URL.");
            }

            return content;
        }

        public static string ValidateUrl(string url)
        {
            if(!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }
            if (!Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                throw new Exception("Invalid URL");
            }

            return url;
        }
    }
}
