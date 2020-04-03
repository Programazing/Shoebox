using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shoebox.Common
{
    public class App
    {
        private readonly ILogger<App> Logger;
        private readonly Settings AppSettings;
        private readonly string CurrentUser;
        public App(IOptionsSnapshot<Settings> appSettings, ILogger<App> logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            AppSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            CurrentUser = AppSettings.UserSettings.CurrentUser;
        }

        public Settings GetSettings() => AppSettings;

        public void Start()
        {
            var user = AppSettings.UserSettings.Users.SingleOrDefault(x => x.UserName == CurrentUser);
            var fileAssociations = user.FileAssociations;
            var sourceFolders = user.WatchedDirectories;

            foreach (var fileAssociation in fileAssociations)
            {
                ProcessFolder(fileAssociation, sourceFolders);
            }
        }

        private void ProcessFolder(FileAssociation input, List<WatchedDirectories> SourceFolders)
        {
            var destinationPath = input.Destination;
            var fileTypes = input.FileTypes.Split(',').Select(FileType => $"*{FileType.Trim()}");

            foreach (var directory in SourceFolders)
            {
                var files = fileTypes.Select(fileType => Directory.GetFiles(directory.Path, fileType));

                foreach (var file in files.SelectMany(collection => collection.Select(file => file)))
                {
                    ProcessFile(input.GetSelectedAction(), destinationPath, file);
                }
            }
        }

        private void ProcessFile(Action selectedAction, string destinationPath, string file)
        {
            try
            {
                if (selectedAction == Action.Move)
                {
                    File.Move(file, Path.Combine(destinationPath, Path.GetFileName(file)));
                }

                if (selectedAction == Action.Copy)
                {
                    File.Copy(file, Path.Combine(destinationPath, Path.GetFileName(file)));
                }

                if (selectedAction == Action.Delete)
                {
                    File.Delete(file);
                }
            }
            catch (IOException ex)
            {
                Logger.LogError(ex, $"Action: {selectedAction}, File: {file}, Exception Message:");

            }
        }
    }
}
