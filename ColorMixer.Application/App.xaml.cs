using ColorMixer.Application.Services;
using ColorMixer.Application.ViewModels;
using ColorMixer.Storage.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ColorMixer.Application.Views;
using System.Windows.Controls;
using System;
using ColorMixer.Contracts.Exceptions;
using System.Windows.Threading;

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
            services.AddSingleton<ViewModelResolver>(serviceProvider => view => 
            {
                ObservableObject viewModel;
                switch (view)
                {
                    case SettingsView settingsView:
                        viewModel = serviceProvider.GetRequiredService<SettingsViewModel>();
                        break;
                    case ColorMixerView colorMixerView:
                        viewModel = serviceProvider.GetRequiredService<ColorMixerViewModel>();
                        break;
                    default: throw new FailedResolveInstanceException("ViewModel is not defined for pointed view (no match in delegate's implementation method)");
                }
                view.DataContext = viewModel;
                return viewModel;
            });
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ColorMixerViewModel>();

            new ColorMixerStorageModule().ConfigureServices(services);
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            MainWindow mainWindow = _provider.GetService<MainWindow>()!;
            ThemeSettingsManager settingsManager = _provider.GetRequiredService<ThemeSettingsManager>();
            await settingsManager.ApplyCurrentThemeSettings();

            mainWindow.Show();
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
        }
    }
}
