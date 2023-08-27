using ColorMixer.Application.Services;
using ColorMixer.Storage.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace ColorMixer.Application
{
    public sealed partial class App : System.Windows.Application
    {
        private readonly ServiceProvider _provider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _provider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ThemeSettingsManager>();

            new ColorMixerStorageModule().ConfigureServices(services);
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = _provider.GetService<MainWindow>()!;
            ThemeSettingsManager settingsManager = _provider.GetRequiredService<ThemeSettingsManager>();
            await settingsManager.ApplyCurrentThemeSettings();

            mainWindow.Show();
        }
    }
}
