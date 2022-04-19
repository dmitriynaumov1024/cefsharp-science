using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text;

namespace Mfetch
{
    public class JsonSerializer
    { 
        public string SerializeToString(object obj)
        {
            StringBuilder sb = new StringBuilder();
            Serialize(obj, new StringWriter(sb));
            return sb.ToString();
        }

        private void Serialize(object obj, TextWriter writer)
        {
            if (obj == null) {
                writer.Write("null");
                return;
            }

            Type objtype = obj.GetType();

            if (objtype.IsPrimitive) {
                if (objtype.Equals(typeof(char))) {
                    if ('\"' == (char)obj)
                        writer.Write("\"\\\"\"");
                    else 
                        writer.Write("\"{0}\"", obj);
                }
                else {
                    writer.Write(obj);
                }
            }

            else if (objtype.Equals(typeof(string))) {
                writer.Write("\"{0}\"", obj.EnsureJsonSafety());
            }

            else if (typeof(IDictionary).IsAssignableFrom(objtype)) {
                writer.Write('{');
                int currentCount = 0;
                IDictionaryEnumerator enumerator = (obj as IDictionary).GetEnumerator();
                enumerator.Reset();
                while (enumerator.MoveNext()) {
                    if (currentCount > 0) writer.Write(", ");
                    Serialize (enumerator.Key, writer);
                    writer.Write(": ");
                    Serialize (enumerator.Value, writer);
                    ++currentCount;
                }
                writer.Write('}');
            }

            else if (typeof(ICollection).IsAssignableFrom(objtype)) {
                writer.Write("[");
                int currentCount = 0;
                foreach (object item in (obj as ICollection)) {
                    if (currentCount > 0) writer.Write(", ");
                    Serialize(item, writer);
                    ++currentCount;
                }
                writer.Write("]");
            }

            else {
                writer.Write('{');
                int currentCount = 0;
                var tmpDictionary = obj.ToDictionary();
                if (tmpDictionary != null) {
                    foreach (var kvPair in tmpDictionary) {
                        if (currentCount > 0) writer.Write(", ");
                        writer.Write("\"{0}\": ", kvPair.Key.EnsureJsonSafety());
                        Serialize(kvPair.Value, writer);
                        ++currentCount;
                    }
                }
                writer.Write('}');
            }
        }
    }
}
