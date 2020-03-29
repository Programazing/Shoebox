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

            Settings = ServiceProvider.GetService<App>().GetSettings();
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

        public void AddUser(User user)
        {
            if(!SettingsContainsUsername(user))
            {
                Settings.UserSettings.Users.Add(user);

                SettingsFile.WriteToSettingsFile(Settings);

                UpdateSettings();
            }
        }

        public void RemoveUser(User user)
        {
            if (SettingsContainsUsername(user))
            {
                var userToRemove = Settings.UserSettings.Users.Single(x => x.UserName == user.UserName);
                Settings.UserSettings.Users.Remove(userToRemove);

                SettingsFile.WriteToSettingsFile(Settings);

                UpdateSettings();
            }
        }

        private void UpdateSettings()
        {
            Settings = ServiceProvider.GetService<App>().GetSettings();
        }

        private bool SettingsContainsUsername(User user) => Settings.UserSettings.Users.Any(x => x.UserName == user.UserName);
    }
}
