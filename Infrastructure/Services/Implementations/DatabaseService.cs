namespace Curacaru.Backend.Infrastructure.Services.Implementations;

using Microsoft.EntityFrameworkCore.Storage;

internal class DatabaseService : IDatabaseService
{
    private readonly DataContext _dataContext;

    public DatabaseService(DataContext dataContext)
        => _dataContext = dataContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
        => _dataContext.Database.BeginTransactionAsync(cancellationToken);
}