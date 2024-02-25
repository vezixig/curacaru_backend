using BudgetReplenisher;
using Curacaru.Backend.Infrastructure;
using Curacaru.Backend.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();
serviceCollection.AddInfrastructure();

var serviceProvider = serviceCollection.BuildServiceProvider();

var budgetRepository = serviceProvider.GetService<IBudgetRepository>()
                       ?? throw new InvalidOperationException("The budget repository is not registered.");
var customerRepository = serviceProvider.GetService<ICustomerRepository>()
                         ?? throw new InvalidOperationException("The customer repository is not registered.");

var worker = new Worker(customerRepository, budgetRepository);
await worker.DoWorkAsync();

Console.WriteLine("-------------");
Console.WriteLine("Work finished");
Console.WriteLine("-------------");