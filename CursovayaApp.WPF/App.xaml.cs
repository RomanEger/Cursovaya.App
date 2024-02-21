using CursovayaApp.WPF.Models.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CursovayaApp.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();
            
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();

            startupForm.Show();

            base.OnStartup(e);
        }

        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, servies) =>
                {
                    servies.AddSingleton<MainWindow>();
                    servies.AddSingleton<DbContext, ApplicationContext>();
                }).
                Build();
        }
    }

}
