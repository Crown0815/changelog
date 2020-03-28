﻿using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Grynwald.ChangeLog.Configuration
{
    internal static class ChangeLogConfigurationLoader
    {
        public const string ConfigurationFileName = "changelog.settings.json";


        public static ChangeLogConfiguration GetConfiguation(string repositoryDirectoy, object? settingsObject = null)
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();

            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(defaultSettingsStream);

            var configurationFilePath = Path.Combine(repositoryDirectoy, ConfigurationFileName);
            if (File.Exists(configurationFilePath))
            {
                // Open file stream and use AddJsonStream() because AddJsonFile() assumes the file name
                // is relative to the ConfigurationBuilder's base directory and does not seem to properly
                // handle absolute paths
                using var configStream = File.Open(configurationFilePath, FileMode.Open, FileAccess.Read);
                builder.AddJsonStream(configStream);

                builder.AddEnvironmentVariables();

                if (settingsObject is object)
                {
                    builder.AddObject(settingsObject);
                }

                return builder.Load();
            }
            else
            {
                builder.AddEnvironmentVariables();

                if (settingsObject is object)
                {
                    builder.AddObject(settingsObject);
                }

                return builder.Load();
            }
        }

        internal static ChangeLogConfiguration GetDefaultConfiguration()
        {
            using var defaultSettingsStream = GetDefaultSettingsStream();

            return new ConfigurationBuilder()
                .AddJsonStream(defaultSettingsStream)
                .Load();
        }


        private static Stream? GetDefaultSettingsStream() =>
            Assembly.GetExecutingAssembly().GetManifestResourceStream("Grynwald.ChangeLog.Configuration.defaultSettings.json");

        private static ChangeLogConfiguration Load(this IConfigurationBuilder builder)
        {
            var configuration = new ChangeLogConfiguration();
            builder
                .Build()
                .GetSection("changelog")
                .Bind(configuration);

            return configuration;
        }
    }
}

