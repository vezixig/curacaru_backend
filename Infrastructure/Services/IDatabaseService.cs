namespace Curacaru.Backend.Infrastructure.Services;

using Microsoft.EntityFrameworkCore.Storage;

public interface IDatabaseService
{
    public Task<IDbContextTransaction> GetTransactionAsync(CancellationToken cancellationToken);
}