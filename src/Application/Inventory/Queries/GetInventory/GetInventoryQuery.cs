using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MusicStore.Application.Common.Interfaces;

namespace MusicStore.Application.Inventory.Queries.GetInventory;

public record GetInventoryQuery : IRequest<IEnumerable<InventoryItem>>;

public class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, IEnumerable<InventoryItem>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetInventoryQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryItem>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AsNoTracking()
            .ProjectTo<InventoryItem>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.Artist)
            .ToListAsync(cancellationToken);
    }
}
