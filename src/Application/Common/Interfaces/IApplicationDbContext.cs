using Microsoft.EntityFrameworkCore;
using MusicStore.Domain.Entities;

namespace MusicStore.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<InventoryItem> InventoryItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
