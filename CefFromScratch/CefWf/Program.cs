using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharpAdvanced;
using Rss;

namespace CefWf
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = 
                System.Globalization.CultureInfo.InvariantCulture;
            Cef.EnableHighDPISupport();
            Cef.Initialize (new CefSettings {});
            
            MainForm form = new MainForm();
            form.Browser.Load(Global.StartPath);
            form.Browser.RegisterMfetchDelegate<string, List<RssItem>>("rss", "get", RssLoader.Instance.GetRss);
            Application.Run(form);
            Cef.Shutdown();
        }
    }
}
