using Application.DataTransferObjects;
using MediatR;

namespace Application.Features.Order.Queries;

public record GetOrderByIdQuery(int Id) : IRequest<OrderDto?>;

