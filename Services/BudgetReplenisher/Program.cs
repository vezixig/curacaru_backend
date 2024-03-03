using BudgetReplenisher;
using Curacaru.Backend.Application;
using Curacaru.Backend.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddInfrastructure();
serviceCollection.AddApplication();

serviceCollection.AddSingleton<Worker>();

var serviceProvider = serviceCollection.BuildServiceProvider();

var worker = serviceProvider.GetRequiredService<Worker>();
await worker.DoWorkAsync();

Console.WriteLine("-------------");
Console.WriteLine("Work finished");
Console.WriteLine("-------------");