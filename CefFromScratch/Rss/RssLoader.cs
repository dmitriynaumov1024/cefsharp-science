using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Rss
{
    public class RssLoader
    {
        // Singleton
        //
        private static RssLoader instance;

        public static RssLoader Instance {
            get {
                if (instance == null) {
                    instance = new RssLoader();
                }
                return instance;
            }
        }

        // Private fields
        //
        private ILogger Log;

        // Constructor
        //
        private RssLoader()
        {
            this.Log = FileLogger.Instance;
        }

        // Methods
        //
        public string GetText (string urlString) 
        {
            return GetXmlDocument(urlString)
                .Then(doc => XmlToRss(doc))
                .Then(rss => RssToString(rss));
        }

        public List<RssItem> GetRss (string urlString)
        {
            return GetXmlDocument(urlString)
                .Then(doc => XmlToRss(doc));
        }

        string RssToString (ICollection<RssItem> rss)
        {
            StringBuilder sb = new StringBuilder();
            foreach (RssItem item in rss) {
                sb.Append(item.Title);
                sb.Append("\r\n- ");
                sb.Append(item.PubDate);
                sb.Append(" -\r\n");
                sb.Append(item.Description);
                sb.Append("\r\n\r\n\r\n");
            }
            Log.Write("RssToString: Done. Total {0} entries.", rss.Count);
            return sb.ToString();
        }

        XmlDocument GetXmlDocument(string urlString)
        {
            urlString = urlString.Trim();
            Uri address;
            Log.Write("");
            Log.Write("GetXmlDocument: Starting...");
            XmlDocument document = new XmlDocument();
            try {
                address = new Uri(urlString, UriKind.Absolute);
            }
            catch (Exception ex) {
                Log.Write("GetXmlDocument: Invalid Uri: {0}", urlString);
                return document;
            }
            Log.Write("GetXmlDocument: Valid Uri: {0}", urlString);
            Log.Write("GetXmlDocument: Creating WebRequest...");
            try {
                HttpWebRequest request = WebRequest.CreateHttp(address);
                request.Proxy = null;
                Log.Write("GetXmlDocument: Sending Request...");
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Log.Write("GetXmlDocument: Parsing Response...");
                Stream source = response.GetResponseStream();
                string chunk = source.ToString();
                if (chunk.Length > 400) chunk = chunk.Substring(0, 400);
                try {
                    Log.Write("GetXmlDocument: Creating XmlDocument...");
                    document.Load(XmlReader.Create(source));
                }
                catch (Exception ex) {
                    string result = String.Format("Exception: That was not Xml:\r\n{0}...", chunk);
                    Log.Write("GetXmlDocument: {0}", result);
                }
                response.Close();
            }
            catch (Exception ex) {
                Log.Write("GetXmlDocument: Can not get anything from: {0}", urlString);
            }
            Log.Write("GetXmlDocument: Done. Returning...");
            return document;
        }

        List<RssItem> XmlToRss (XmlDocument document) 
        {
            List<RssItem> result = new List<RssItem>();
            if(document.DocumentElement == null || document.DocumentElement.IsEmpty) { 
                Log.Write("XmlToRss: XmlDocument is empty.");
                return result;
            }
            var nodes = document.DocumentElement.SelectNodes("/rss/channel/item");
            foreach (object xmlNodeObj in nodes) {
                XmlNode node = xmlNodeObj as XmlNode;
                try {
                    result.Add(new RssItem {
                        Title = node["title"].InnerText.RemoveXmlTags(),
                        PubDate = node["pubDate"].InnerText,
                        Description = node["description"].InnerText.RemoveXmlTags()
                    });
                }
                catch (Exception ex) {
                    continue;
                }
            }
            Log.Write("XmlToRss: Done. {0} entries", result.Count);
            return result;
        }
    }
}
