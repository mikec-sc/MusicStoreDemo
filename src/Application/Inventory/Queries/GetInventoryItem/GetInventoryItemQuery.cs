using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicStore.Application.Common.Interfaces;
using MusicStore.Application.Common.Models;

namespace MusicStore.Application.Inventory.Queries.GetInventory;

public record GetInventoryItemQuery : IRequest<InventoryItem>
{
    public int Id { get; init; }
}

public class GetInventoryItemQueryHandler : IRequestHandler<GetInventoryItemQuery, InventoryItem>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInventoryItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<InventoryItem?> Handle(GetInventoryItemQuery request, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .Where(item => item.Id == request.Id) // Filter by ID
            .ProjectTo<InventoryItem>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
