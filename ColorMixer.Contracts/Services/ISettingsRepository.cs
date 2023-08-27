using System;
using System.Threading.Tasks;
using ColorMixer.Contracts.Exceptions;
using ColorMixer.Contracts.Helpers;

namespace ColorMixer.Contracts.Services
{
    /// <summary>
    /// Responsible for settings persistence and updating.
    /// </summary>
    public interface ISettingsRepository
    {
        /// <summary>
        /// Get actual setting value from the storage.
        /// </summary>
        /// <typeparam name="T">Setting type (string or bool)</typeparam>
        /// <param name="settingKey">Key of a required setting <see cref="SettingsHelper"/>.</param>
        /// <throws><see cref="SettingDoesNotExistException"/>In case setting with specified key doesn't exist in the storage.</throws>
        /// <throws><see cref="ArgumentException"/>Setting ket is null or unsupported type T.</throws>
        /// <returns>Deserialed setting value</returns>
        public Task<T> GetSetting<T> (string settingKey);
        /// <summary>
        /// Updates setting value.
        /// </summary>
        /// <typeparam name="T">Type of setting (string or bool).</typeparam>
        /// <param name="settingKey">Key of a setting to update <see cref="SettingsHelper"/>.</param>
        /// <param name="value">Value</param>
        /// <throws><see cref="SettingDoesNotExistException"/> in case setting with specified key doesn't exist in the storage.</throws>
        /// <throws><see cref="ArgumentException"/>Setting ket is null or unsupported type T.</throws>
        /// <returns>Updated setting value</returns>
        public Task<T> SaveSetting<T>(string settingKey, T value);
    }
}
