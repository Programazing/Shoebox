using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Shoebox.Common
{
    public static class SettingsFile
    {
        public static string FileName = "appsettings.json";
        public static string SettingsPath;

        public static void SetValidPath(string path)
        {
            var filePath = string.IsNullOrEmpty(path) ? DefaultPath() : path;

            SettingsPath = filePath;

            if (!FileExists(filePath)) { CreateSettingsFile(); } 
        }
        private static void CreateSettingsFile()
        {
            var folder = Directory.GetParent(SettingsPath).ToString();

            if (!DirectoryExists(folder)) { Directory.CreateDirectory(Directory.GetParent(folder).ToString()); }

            if (!FileExists(SettingsPath))
            {
                WriteToSettingsFile(DefaultSettings());
            }
        }

        private static void WriteToSettingsFile(Settings settings)
        {
            string jsonString = JsonSerializer.Serialize(settings, JsonOptions);

            File.WriteAllText(SettingsPath, jsonString);
        }

        private static string DefaultPath() => Path.Combine(Directory.GetCurrentDirectory(), FileName);
        private static Settings DefaultSettings()
        {
            var appSettings = new AppSettings();
            var watchedDirectories = new List<WatchedDirectories> { new WatchedDirectories() { Path = "" } };
            var fileAssociation = new FileAssociations() { Action = "Copy", Destination = "", FileTypes = "", Name = "" };
            var fileAssociations = new List<FileAssociations> { fileAssociation };
            var users = new List<User> { new User() { UserName = "DefaultUser", FileAssociations = fileAssociations, WatchedDirectories = watchedDirectories }};
            var userSettings = new UserSettings() { Users = users };
            var settings = new Settings() { AppSettings = appSettings, UserSettings = userSettings };

            return settings;
        }

        public static void AddUser(Settings settings)
        {
            WriteToSettingsFile(settings);
        }

        private static bool FileExists(string path) => File.Exists(path);
        private static bool DirectoryExists(string path) => Directory.Exists(path);
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }
}
