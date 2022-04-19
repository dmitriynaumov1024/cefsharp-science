using System;

namespace Rss
{
    interface ILogger
    {
        void Write(string format, params object[] args);
    }
}
