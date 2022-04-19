using System;

namespace Rss
{
    // Applicable to CefSharp 57
    // With many trials and errors, we have determined
    // that to return complex entity from an async method, 
    // it must be a struct with plain fields, not computed props
    public class RssItem
    {
        public string Title { get; set; }
        public string PubDate { get; set; }
        public string Description { get; set; }
    }
}
