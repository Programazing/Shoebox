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

        public static string GetValidPath(string path)
        {
            var filePath = string.IsNullOrEmpty(path) ? DefaultPath() : path;

            if (!FileExists(filePath)) { CreateSettingsFile(filePath); }

            return filePath;
        }
        private static void CreateSettingsFile(string filePath)
        {
            var folder = Directory.GetParent(filePath).ToString();

            if (!DirectoryExists(folder)) { Directory.CreateDirectory(Directory.GetParent(folder).ToString()); }

            if (!FileExists(filePath))
            {
                string jsonString = JsonSerializer.Serialize(DefaultSettings(), JsonOptions);

                File.WriteAllText(filePath, jsonString);
            }
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
        private static bool FileExists(string path) => File.Exists(path);
        private static bool DirectoryExists(string path) => Directory.Exists(path);
        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
    }
}
