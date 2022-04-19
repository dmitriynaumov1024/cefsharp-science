using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CefSharp.WinForms;
using Mfetch;

namespace CefSharpAdvanced
{
    public class ChromiumWebBrowser: CefSharp.WinForms.ChromiumWebBrowser
    {
        MfetchObject mfetch;

        public ChromiumWebBrowser(string address) : base(address) { }

        public void RegisterMfetchObject(string name, object obj)
        {
            this.EnsureMfetchExists();
            // this.mfetch.AddDelegate
        }

        public void RegisterMfetchDelegate<TOut>(string name, string subname, Func<TOut> f) where TOut: class
        {
            this.EnsureMfetchExists();
            this.mfetch.AddDelegate(name+"."+subname, f);
        }

        public void RegisterMfetchDelegate<TIn, TOut>(string name, string subname, Func<TIn, TOut> f) where TOut: class
        {
            this.EnsureMfetchExists();
            this.mfetch.AddDelegate(name+"."+subname, f);
        }

        private void EnsureMfetchExists()
        {
            if (this.mfetch == null)
            {
                this.mfetch = new MfetchObject();
                this.RegisterAsyncJsObject("m", this.mfetch.SafeAdapter);
            }
        }
    }
}
