using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shoebox.Common
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly Settings _appSettings;

        public App(IOptionsSnapshot<Settings> appSettings, ILogger<App> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public Settings GetSettings()
        {
            return _appSettings;
        }

    }
}
