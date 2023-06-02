using Globomantics.Domain;
using Globomantics.Infrastructure.Data;
using Globomantics.Infrastructure.Data.Repositories;
using Globomantics.Windows.Factories;
using Globomantics.Windows.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace Globomantics.Windows;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static User CurrentUser { get; set; } = default!;

    public IServiceProvider ServiceProvider { get; init; } 
    public IConfiguration Configuration { get; init; }

    public App()
    {
        var builder = new ConfigurationBuilder();

        Configuration = builder.Build();

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<GlobomanticsDbContext>(ServiceLifetime.Scoped);

        //serviceCollection.AddSingleton<IRepository<TodoTask>, TodoInMemoryRepository>();
        serviceCollection.AddSingleton<IRepository<Bug>, BugRepository>();
        serviceCollection.AddSingleton<IRepository<Feature>, FeatureRepository>();
        serviceCollection.AddSingleton<IRepository<User>, UserRepository>();
        serviceCollection.AddSingleton<IRepository<TodoTask>, TodoTaskRepository>();

        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<TodoViewModelFactory>();
        serviceCollection.AddTransient<BugViewModel>();
        serviceCollection.AddTransient<FeatureViewModel>();
        serviceCollection.AddTransient<MainWindow>();

        serviceCollection.AddTransient(_ => ServiceProvider!);
        
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    private void SeedWithData()
    {
        var context = ServiceProvider.GetRequiredService<GlobomanticsDbContext>();

        if (context.Users.Any()) return;

        context.Users.Add(new Infrastructure.Data.Models.User { Name = "Filip" });
        context.Users.Add(new Infrastructure.Data.Models.User { Name = "Sofie" });
        context.Users.Add(new Infrastructure.Data.Models.User { Name = "Mila" });
        context.Users.Add(new Infrastructure.Data.Models.User { Name = "Elise" });

        context.SaveChanges();
    }

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        try
        {
            var context = ServiceProvider.GetRequiredService<GlobomanticsDbContext>();

            await context.Database.MigrateAsync();

            SeedWithData();
        }
        catch(Exception)
        {
            throw;
        }

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow?.Show();
    }
}
