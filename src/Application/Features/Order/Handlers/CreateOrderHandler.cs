using Application.Contracts;
using Application.DataTransferObjects;
using Application.Features.Order.Commands;
using AutoMapper;
using Domain.Entities;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Order.Handlers;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ErrorOr<string>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public CreateOrderHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ErrorOr<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // Check product types
        var requestProductIds = request.Products.Select(q => q.ProductId);
        var products = await _context.ProductTypes.Where(q => requestProductIds.Contains(q.Id))
            .ToListAsync(cancellationToken);

        var invalidProductIds = requestProductIds.Except(products.Select(x => x.Id)).ToList();
        if (invalidProductIds.Any())
            return Error.Validation($"Invalid product type(s) found: {string.Join(", ", invalidProductIds)}");

        var order = new Domain.Entities.Order
        {
            Date = DateTimeOffset.UtcNow,
            Products = _mapper.Map<List<OrderedProduct>>(request.Products),
            RequiredBinWidth = CalculateMinimumBinWidth(request.Products, products)
        };

        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return $"OrderID: {order.Id}, please prepare a bin with minimum width of {order.RequiredBinWidth:0.##} mm.";
    }

    private static double CalculateMinimumBinWidth(IEnumerable<OrderedProductCreateDto> requestProducts,
        IEnumerable<ProductType> products)
    {
        var productQuery = from product in products
                           join requestProduct in requestProducts
                               on product.Id equals requestProduct.ProductId
                           select new
                           {
                               product.StackSize,
                               product.RequiredBinWidth,
                               requestProduct.Quantity
                           };

        return productQuery.Sum(q => Math.Ceiling((double)q.Quantity / q.StackSize) * q.RequiredBinWidth);
    }
}