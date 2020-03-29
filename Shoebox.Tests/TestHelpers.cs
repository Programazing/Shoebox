using Shoebox.Common;
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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), SettingsFile.FileName);
        }

        public static List<User> Users()
        {
            var watchedDirectories = new List<WatchedDirectories> { new WatchedDirectories() { Path = "" } };
            var fileAssociation = new FileAssociations() { Action = "Copy", Destination = "", FileTypes = "", Name = "" };
            var fileAssociations = new List<FileAssociations> { fileAssociation };
            return new List<User>
            { 
                new User() { UserName = "DefaultUser", FileAssociations = fileAssociations, WatchedDirectories = watchedDirectories },
                new User() { UserName = "JohnDoe", FileAssociations = fileAssociations, WatchedDirectories = watchedDirectories }
            };
        }

        private static Settings DefaultSettings()
        {
            var appSettings = new AppSettings();
            var userSettings = new UserSettings() { Users = Users() };
            var settings = new Settings() { AppSettings = appSettings, UserSettings = userSettings };

            return settings;
        }
    }
}
