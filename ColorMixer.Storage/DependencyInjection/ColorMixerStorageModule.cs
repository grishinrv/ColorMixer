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
        private string _dataSource;
        private string _appDataColorMixerFolder;
        private string _appDataColorMixerStorageFile;
        private const string STORAGE_FILE_NAME = "Storage.db";

#pragma warning disable CS8618 
        public ColorMixerStorageModule()
#pragma warning restore CS8618 
        {
            InsureStorageFileExists();
            CopyInitialDbFileToAppData();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
            services.AddDbContextFactory<AppDbContext>(o => o.UseSqlite(_dataSource));
            services.AddDbContextFactory<SettingsDbContext>(o => o.UseSqlite(_dataSource));
        }

        /// <summary>
        /// Used for migrations. Do not remove.
        /// </summary>
        private void InsureStorageFileExists()
        {
            _appDataColorMixerFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ColorMixer");
            if (!Directory.Exists(_appDataColorMixerFolder))
                Directory.CreateDirectory(_appDataColorMixerFolder);
            _appDataColorMixerStorageFile = Path.Combine(_appDataColorMixerFolder, STORAGE_FILE_NAME);
            if (!File.Exists(_appDataColorMixerStorageFile))
                File.Create(_appDataColorMixerStorageFile);

            _dataSource = $"Data Source={_appDataColorMixerStorageFile}";
        }

        /// <summary>
        /// On first application launch - move storage gile to <see cref="_appDataColorMixerStorageFile"/>
        /// </summary>
        private void CopyInitialDbFileToAppData()
        {
            string defaultStorageFile = Path.Combine(AppContext.BaseDirectory, STORAGE_FILE_NAME);
            if (File.Exists(defaultStorageFile))
                File.Move(defaultStorageFile, _appDataColorMixerStorageFile);
        }
    }
}
