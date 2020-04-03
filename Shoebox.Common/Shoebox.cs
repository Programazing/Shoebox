using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shoebox.Common
{
    public class Shoebox
    {        
        private ServiceProvider ServiceProvider;
        public Settings Settings;

        public Shoebox(string filePath = "")
        {
            SetFields(filePath);

            BuildServices();
        }

        private void SetFields(string filePath)
        {
            SettingsFile.SetValidPath(filePath);
        }

        private void BuildServices()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            ServiceProvider = services.BuildServiceProvider();

            RefreshSettings();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
                builder
                    .AddDebug()
                    .AddConsole()
            );

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(SettingsFile.SettingsPath).ToString())
                .AddJsonFile(SettingsFile.FileName, optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<Settings>(configuration);
            services.AddOptions();

            services.AddTransient<App>();
        }

        public void ChangeCurrentUser(User user)
        {
            Settings.UserSettings.CurrentUser = user.UserName;

            UpdateSettings();
        }

        public void Start() => ServiceProvider.GetService<App>().Start();

        public void AddUser(User user)
        {
            if(!SettingsContainsUsername(user))
            {
                Settings.UserSettings.Users.Add(user);
                UpdateSettings();
            }
        }

        public void RemoveUser(User user)
        {
            if (SettingsContainsUsername(user))
            {
                var userToRemove = GetUserFromSettings(user);
                Settings.UserSettings.Users.Remove(userToRemove);

                UpdateSettings();
            }
        }

        public void UpdateUser(User user)
        {
            if (SettingsContainsUsername(user))
            {
                var index = Settings.UserSettings.Users.FindIndex(x => x.UserName == user.UserName);

                Settings.UserSettings.Users[index] = user;

                UpdateSettings();
            }
        }

        private User GetUserFromSettings(User user) => Settings.UserSettings.Users.Single(x => x.UserName == user.UserName);

        private void UpdateSettings()
        {
            SettingsFile.WriteToSettingsFile(Settings);

            RefreshSettings();
        }

        private void RefreshSettings() => Settings = ServiceProvider.GetService<App>().GetSettings();

        private bool SettingsContainsUsername(User user) => Settings.UserSettings.Users.Any(x => x.UserName == user.UserName);

    }
}
