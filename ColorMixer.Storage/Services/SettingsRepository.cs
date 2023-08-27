using ColorMixer.Contracts.Exceptions;
using ColorMixer.Contracts.Services;
using ColorMixer.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ColorMixer.Storage.Services
{
    public sealed class SettingsRepository : ISettingsRepository
    {
        private readonly IDbContextFactory<SettingsDbContext> _contextFactory;
        public SettingsRepository(IDbContextFactory<SettingsDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> GetSetting<T>(string settingKey)
        {
            ValidateType<T>();
            ArgumentNullException.ThrowIfNull(settingKey, nameof(settingKey));

            await using SettingsDbContext context = await _contextFactory.CreateDbContextAsync();
            try
            {
                SettingsModel setting = await context.Settings.AsNoTracking().FirstAsync(x => x.Key == settingKey);
                return (T)Convert.ChangeType(setting.Value, typeof(T))!;
            }
            catch (NullReferenceException)
            {
                throw new SettingDoesNotExistException(settingKey);
            }
        }

        public async Task<bool> SaveSettingIfChanged<T>(string settingKey, T value)
        {
            ValidateType<T>();
            ArgumentNullException.ThrowIfNull(settingKey, nameof(settingKey));

            await using SettingsDbContext context = await _contextFactory.CreateDbContextAsync();
            try
            {
                SettingsModel setting = await context.Settings.FirstAsync(x => x.Key == settingKey);
                if (!string.Equals(setting.Value, value?.ToString()))
                {
                    setting.Value = value?.ToString();
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (NullReferenceException)
            {
                throw new SettingDoesNotExistException(settingKey);
            }
        }

        private void ValidateType<T>()
        {
            if (typeof(T) != typeof(string) && typeof(T) != typeof(bool))
                throw new ArgumentException($"Unsupported setting type: \"{typeof(T)}\".");
        }
    }
}
