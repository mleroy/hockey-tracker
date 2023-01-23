using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace StatsParser
{
    class Helpers
    {
        public static string GetHtml(string url)
        {
            string html = string.Empty;

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    html = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception for " + url + ": " + e.ToString());
                return null;
            }

            return html;
        }
    }
}
