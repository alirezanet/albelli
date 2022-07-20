using Application.Contracts;
using Application.DataTransferObjects;
using Application.Features.Order.Handlers;
using Application.Features.Order.Queries;
using Application.Mappings;
using AutoBogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Application.Test.Order;

public class GetOrderByIdHandlerTest : IClassFixture<MapperFixture<OrderMappings>>
{
    private readonly IAppDbContext _context;
    private readonly GetOrderByIdHandler _sut;

    public GetOrderByIdHandlerTest(MapperFixture<OrderMappings> mapperFixture)
    {
        _context = Substitute.For<IAppDbContext, DbContext>();
        var mapper = mapperFixture.Mapper;
        _sut = new GetOrderByIdHandler(_context, mapper);
    }

    [Fact]
    public async Task GetOrderById_IfOrderNotExist_ShouldReturnNull()
    {
        // Arrange
        var query = new GetOrderByIdQuery(4);

        var dbSet = new AutoFaker<Domain.Entities.Order>()
            .RuleFor(q => q.Id, 1)
            .Generate(1)
            .ToMockDbSet();

        _context.Orders.Returns(dbSet);

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderById_IfOrderExist_ShouldReturnOrderDto()
    {
        // Arrange
        var query = new GetOrderByIdQuery(1);

        var dbSet = new AutoFaker<Domain.Entities.Order>()
            .RuleFor(q => q.Id, 1)
            .Generate(1)
            .ToMockDbSet();

        _context.Orders.Returns(dbSet);

        // Act
        var result = await _sut.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<OrderDto>();
        result!.Id.Should().Be(1);
        result!.RequiredBinWidth.Should().NotBe(0);
    }
}