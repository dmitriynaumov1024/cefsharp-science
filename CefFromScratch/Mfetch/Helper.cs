using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mfetch
{
    static class Helper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            if (source == null) return null;
            var dict = new SortedDictionary<string, object>();
            var props = source.GetType().GetProperties (
                BindingFlags.Instance | BindingFlags.Public
            );
            foreach (PropertyInfo property in props) {
                if (property.CanRead) {
                    dict.Add(property.Name, property.GetValue(source));
                }
            }
            return dict;
        }

        public static object EnsureJsonSafety(this object obj)
        {
            if(obj.GetType() == typeof(String))
            {
                obj = (obj as string).Replace("\"","\\\"");
            }
            return obj;
        }
    }
}
