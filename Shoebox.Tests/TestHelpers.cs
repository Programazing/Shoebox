using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shoebox.Tests
{
    public static class TestHelpers
    {
        public static string DesktopLocation()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Common.SettingsFile.FileName);
        }
    }
}
