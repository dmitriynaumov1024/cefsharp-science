﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefWf
{
    static class Global
    {
        public static string WorkDir = System.IO.Directory.GetCurrentDirectory().Replace('\\', '/');
        public static string StartPath = WorkDir + "/frontend/index.html";
    }
}
