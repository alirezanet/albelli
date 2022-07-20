using Application.DataTransferObjects;
using MediatR;

namespace Application.Features.Product.Queries;

public record GetProductsQuery : IRequest<IList<ProductDto>>;
 