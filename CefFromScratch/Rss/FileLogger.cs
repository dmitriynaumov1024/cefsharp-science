using System;
using System.IO;

namespace Rss
{
    class FileLogger : ILogger
    {
        // Singleton
        //
        private static FileLogger instance;

        public static FileLogger Instance {
            get {
                if (instance==null) {
                    instance = new FileLogger();
                }
                return instance;
            }
        }

        // Fields
        //
        private StreamWriter writer;

        // Constructor
        //
        private FileLogger()
        {
            writer = new StreamWriter(new FileStream("Log-"+DateId+".log", FileMode.Append));
            writer.AutoFlush = true;
        }

        // Methods
        //
        public void Write (string format, params object[] args)
        {
            writer.Write("[{0}] ", Timestamp);
            writer.Write(format, args);
            writer.Write("\r\n");
        }

        public static string Timestamp {
            get {
                return String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.UtcNow);
            }
        }

        static string DateId {
            get {
                return String.Format("{0:yyyy-MM-dd}", DateTime.UtcNow);
            }
        }
    }
}
