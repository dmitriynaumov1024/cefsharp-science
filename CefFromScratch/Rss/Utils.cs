using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Rss
{
    static class Utils
    {
        static Regex RE_XML_TAGS = new Regex(@"\<[^\>]+\>");
        static string[] Whitespaces = new string[]{" ", "\t", "\n", "\r"};
        static StringSplitOptions NoEmpty = StringSplitOptions.RemoveEmptyEntries;

        public static string RemoveXmlTags (this string source)
        {
            string[] words = RE_XML_TAGS
                .Replace(source, " ")
                .HtmlDecode()
                .Split(Whitespaces, NoEmpty);
            return String.Join(" ", words);
        }

        static string HtmlDecode (this string source)
        {
            return WebUtility.HtmlDecode(source);
        }

        public static T2 Then<T1, T2> (this T1 obj, Func<T1, T2> func) 
            where T1: class 
            where T2: class
        {
            return func(obj);
        }
    }
}
