using ColorMixer.Contracts.DependecyInjection;
using ColorMixer.Contracts.Services;
using ColorMixer.Storage.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System;

namespace ColorMixer.Storage.DependencyInjection
{
    public class ColorMixerStorageModule : IAppModule
    {
        private readonly string _dataSource;
        private readonly string _appDataColorMixerFolder;
        private readonly string _appDataColorMixerStorageFile;

        public ColorMixerStorageModule()
        {
            _appDataColorMixerFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ColorMixer");
            if (!Directory.Exists(_appDataColorMixerFolder))
                Directory.CreateDirectory(_appDataColorMixerFolder);
            _appDataColorMixerStorageFile = Path.Combine(_appDataColorMixerFolder, "Storage.db");
            if (!File.Exists(_appDataColorMixerStorageFile))
                File.Create(_appDataColorMixerStorageFile);

            _dataSource = $"Data Source={_appDataColorMixerStorageFile}";
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            services.AddDbContextFactory<AppDbContext>(o => o.UseSqlite(_dataSource));
            services.AddDbContextFactory<SettingsDbContext>(o => o.UseSqlite(_dataSource));
        }
    }
}
