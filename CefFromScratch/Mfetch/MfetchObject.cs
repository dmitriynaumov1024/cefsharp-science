using System;
using System.Collections.Generic;
using System.Text;

namespace Mfetch
{
    public class MfetchObjectSafeAdapter
    {
        private MfetchObject target;

        public MfetchObjectSafeAdapter(MfetchObject obj)
        {
            this.target = obj;
        }

        public string Fetch(string name, string paramsJsonArray)
        {
            return target.Fetch(name, paramsJsonArray);
        }
    }

    public class MfetchObject
    {
        SortedDictionary<string, Delegate> targetDelegates;
        SortedDictionary<string, Func<string, string>> bridgeDelegates;
        MfetchObjectSafeAdapter adapter;
        JsonSerializer serializer;

        public MfetchObject()
        {
            this.targetDelegates = new SortedDictionary<string, Delegate>();
            this.bridgeDelegates = new SortedDictionary<string, Func<string, string>>();
            this.adapter = new MfetchObjectSafeAdapter(this);
            this.serializer = new JsonSerializer();
        }

        public MfetchObjectSafeAdapter SafeAdapter
        {
            get { return this.adapter; }
        }

        public string Fetch (string name, string paramsJsonArray)
        {
            Func<string, string> bridgeDlg = this.bridgeDelegates[name];
            if (bridgeDlg == null) return "{}";
            return bridgeDlg(paramsJsonArray);
        }

        public void AddDelegate<TOut> (string name, Func<TOut> func) where TOut: class
        {
            this.targetDelegates[name] = func;
            this.bridgeDelegates[name] = (input) => {
                Func<TOut> f = (Func<TOut>)this.targetDelegates[name];
                TOut result = f();
                return result.ToString();
            };
        }

        public void AddDelegate<TIn, TOut> (string name, Func<TIn, TOut> func) where TOut: class
        {
            this.targetDelegates[name] = func;
            this.bridgeDelegates[name] = (input) => {
                Delegate f = this.targetDelegates[name];
                TOut result = TryInvoke<TOut>(f, new[]{ input });
                return serializer.SerializeToString(result);
            };
        }

        public void AddDelegate<TOut> (string name, Delegate func) where TOut: class
        {
            this.targetDelegates[name] = func;
            this.bridgeDelegates[name] = (input) => {
                Delegate f = this.targetDelegates[name];
                TOut result = TryInvoke<TOut>(f, input.Split(';'));
                return serializer.SerializeToString(result);
            };
        }

        static TOut TryInvoke<TOut> (Delegate dlg, object[] args) where TOut: class
        {
            if (dlg == null) return null;
            return (TOut)dlg.DynamicInvoke(args);
        }

        string a (string address, int count)
        {
            return "aaa" + address + count.ToString();
        }
    }
}
