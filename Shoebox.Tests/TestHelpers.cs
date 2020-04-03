using Shoebox.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shoebox.Tests
{
    public static class TestHelpers
    {
        static string CurrentPath;
        static string John;
        static string Desktop;
        static string Docs;
        public static string Downloads;
        static string Pics;

        static TestHelpers()
        {
            CurrentPath = Directory.GetCurrentDirectory();
            John = Path.Combine(CurrentPath, "John");

            Desktop = Path.Combine(John, "Desktop");
            Docs = Path.Combine(John, "Documents");
            Downloads = Path.Combine(John, "Downloads");
            Pics = Path.Combine(John, "Pictures");
        }

        public static string DesktopLocation()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), SettingsFile.FileName);
        }

        public static List<User> Users()
        {
            var watchedDirectories = new List<WatchedDirectories> { new WatchedDirectories() { Path = "" } };
            var fileAssociation = new FileAssociation() { Action = "Copy", Destination = "", FileTypes = "", Name = "" };
            var fileAssociations = new List<FileAssociation> { fileAssociation };
            return new List<User>
            {
                new User() { UserName = "DefaultUser", FileAssociations = fileAssociations, WatchedDirectories = watchedDirectories },
                GetJohnDoe()
            };
        }

        public static User GetJohnDoe()
        {
            var watchedDirectories = new List<WatchedDirectories>
            {
                new WatchedDirectories() { Path = Downloads },
                new WatchedDirectories() { Path = Desktop }
            };
            var fileAssociations = new List<FileAssociation>
            {
                new FileAssociation() { Action = "Move", Destination = "D:\\P.Desktop\\John\\Pictures", FileTypes = ".jpg", Name = "Images" },
                new FileAssociation() { Action = "Copy", Destination = "D:\\P.Desktop\\John\\Documents", FileTypes = ".txt", Name = "Documents" }
            };
            return new User() { UserName = "JohnDoe", FileAssociations = fileAssociations, WatchedDirectories = watchedDirectories };
        }

        private static Settings DefaultSettings()
        {
            var appSettings = new AppSettings();
            var userSettings = new UserSettings() { Users = Users() };
            var settings = new Settings() { AppSettings = appSettings, UserSettings = userSettings };

            return settings;
        }

        public static void CreateFoldersForJohn()
        {
            if(!Directory.Exists(John))
            {
                Directory.CreateDirectory(John);
                Directory.CreateDirectory(Desktop);
                Directory.CreateDirectory(Docs);
                Directory.CreateDirectory(Downloads);
                Directory.CreateDirectory(Pics);

                File.WriteAllText(Path.Combine(Downloads, "test.txt"), "content");
            }
        }

        public static void DeleteFoldersForJohn()
        {
            var path = Directory.GetCurrentDirectory();
            var john = Path.Combine(path, "John");

            if (Directory.Exists(john))
            {
                Directory.Delete(john, recursive: true);
            }
        }
    }
}
