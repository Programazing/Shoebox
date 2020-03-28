using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Shoebox.Common
{
    public class Shoebox
    {
        private string SettingsPath;
        private ServiceProvider ServiceProvider;
        public Settings Settings;

        public Shoebox(string filePath = "")
        {
            SetFields(filePath);

            BuildServices();

        }

        private void SetFields(string filePath)
        {
            SettingsPath = SettingsFile.GetValidPath(filePath);

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
                .SetBasePath(Directory.GetParent(SettingsPath).ToString())
                .AddJsonFile(SettingsFile.FileName, optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.Configure<Settings>(configuration);
            services.AddOptions();

            services.AddTransient<App>();

        }
    }
}
