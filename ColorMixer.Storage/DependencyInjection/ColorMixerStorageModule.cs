using ColorMixer.Contracts.DependecyInjection;
using ColorMixer.Contracts.Services;
using ColorMixer.Storage.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ColorMixer.Storage.DependencyInjection
{
    public class ColorMixerStorageModule : IAppModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISettingsRepository, SettingsRepository>();
        }
    }
}
