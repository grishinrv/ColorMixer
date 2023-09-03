using ColorMixer.Application.Services;
using ColorMixer.Application.ViewModels;
using ColorMixer.Storage.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using ColorMixer.Application.Views;
using ColorMixer.Contracts.Exceptions;
using System.Windows.Threading;
using System;
using System.Threading.Tasks;
using System.Diagnostics;

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
            services.AddSingleton<ViewManager>();
            services.AddSingleton<IViewManager>(s => s.GetService<ViewManager>()!);

            services.AddTransient<SettingsViewModel>();
            services.AddTransient<ColorMixerViewModel>();

            new ColorMixerStorageModule().ConfigureServices(services);
        }

        private async void OnStartup(object sender, StartupEventArgs e)
        {
            Dispatcher.UnhandledException += Dispatcher_UnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            MainWindow mainWindow = _provider.GetService<MainWindow>()!;
            ThemeSettingsManager settingsManager = _provider.GetRequiredService<ThemeSettingsManager>();
            await settingsManager.ApplyCurrentThemeSettings();

            mainWindow.Show();
        }

        private void Dispatcher_UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // todo log
            Debugger.Break();
            Environment.Exit(1);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // todo log
            Debugger.Break();
            Environment.Exit(1);
        }

        private void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            // todo log
            Debugger.Break();
            Environment.Exit(1);
        }
    }
}
