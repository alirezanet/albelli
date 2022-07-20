using Application.Contracts;
using Application.DataTransferObjects;
using Application.Features.Product.Queries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Product.Handlers;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, IList<ProductDto>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetProductsHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IList<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.ProductTypes
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}