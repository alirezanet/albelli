using Application.DataTransferObjects;
using ErrorOr;
using MediatR;

namespace Application.Features.Order.Commands;

public record CreateOrderCommand(IList<OrderedProductCreateDto> Products) : IRequest<ErrorOr<string>>;