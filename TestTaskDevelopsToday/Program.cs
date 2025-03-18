using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using TestTaskDevelopsToday.Abstractions;
using TestTaskDevelopsToday.Data;
using TestTaskDevelopsToday.Implementations;
using Microsoft.Extensions.Hosting;

namespace TestTaskDevelopsToday;

class Program(UserInterface userInterface)
{
    static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.Services.GetRequiredService<Program>().Run();
    }

    private async Task Run()
    {
        await userInterface.Start();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddTransient<Program>();
                services.AddTransient<IRepository, Repository>();
                services.AddTransient<SqlConnectionFactory>();
                services.AddSingleton<UserInterface>();
            });
    }
}