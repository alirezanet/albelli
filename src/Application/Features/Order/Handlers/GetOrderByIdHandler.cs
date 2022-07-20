using Application.Contracts;
using Application.DataTransferObjects;
using Application.Features.Order.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Order.Handlers;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetOrderByIdHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        => await _context.Orders
            .Where(q => q.Id == query.Id)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(cancellationToken);
}