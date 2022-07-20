using Application.Contracts;
using Application.DataTransferObjects;
using Application.Features.Order.Commands;
using Application.Features.Order.Handlers;
using Application.Mappings;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace Application.Test.Order;

public class CreateOrderHandlerTest : IClassFixture<MapperFixture<OrderMappings>>
{
    private readonly IAppDbContext _context;
    private readonly CreateOrderHandler _sut;

    public CreateOrderHandlerTest(MapperFixture<OrderMappings> mapperFixture)
    {
        _context = Substitute.For<IAppDbContext, DbContext>();
        var mapper = mapperFixture.Mapper;
        _sut = new CreateOrderHandler(_context, mapper);
    }

    [Fact]
    public async Task CreateOrder_ShouldSaveOrderAndReturnRequiredBinWidth()
    {
        // Arrange
        var products = new[]
        {
            new OrderedProductCreateDto(1, 2),
            new OrderedProductCreateDto(5, 4)
        };
        var command = new CreateOrderCommand(products);

        var dbSet = GetTestProductTypes().ToMockDbSet();
        _context.ProductTypes.Returns(dbSet);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        await _context.Orders.Received().AddAsync(Arg.Any<Domain.Entities.Order>());
        await _context.Received().SaveChangesAsync(default);
        result.Value.Should().Be($"OrderID: 0, please prepare a bin with minimum width of 132 mm.");
    }


    [Fact]
    public async Task CreateOrder_WithInvalidProductIds_ShouldReturnValidationError()
    {
        // Arrange
        var product = new[]
        {
            new OrderedProductCreateDto(1, 2),
            new OrderedProductCreateDto(10, 3) // invalid product id (10)
        };
        var command = new CreateOrderCommand(product);

        var dbSet = GetTestProductTypes().ToMockDbSet();
        _context.ProductTypes.Returns(dbSet);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be("Invalid product type(s) found: 10");
    }

    [Theory]
    [MemberData(nameof(CalculatorData))]
    public async Task CreateOrder_RequiredBinWidth_Calculation(List<OrderedProductCreateDto> products, double binWidth)
    {
        // Arrange
        var command = new CreateOrderCommand(products);

        var dbSet = GetTestProductTypes().ToMockDbSet();
        _context.ProductTypes.Returns(dbSet);

        // Act
        var result = await _sut.Handle(command, default);

        // Assert
        result.IsError.Should().BeFalse();
        await _context.Orders.Received().AddAsync(Arg.Any<Domain.Entities.Order>());
        await _context.Received().SaveChangesAsync(default);
        result.Value.Should().Be($"OrderID: 0, please prepare a bin with minimum width of {binWidth} mm.");
    }


    #region TestData
    public static IEnumerable<object[]> CalculatorData()
    {
        yield return new object[]
        {
            new List<OrderedProductCreateDto>()
            {
                new(1, 6),
            },
            114
        };

        yield return new object[]
        {
            new List<OrderedProductCreateDto>()
            {
                new(5, 6),
            },
            188
        };

        yield return new object[]
        {
            new List<OrderedProductCreateDto>()
            {
                new(1, 1),
                new(2, 1),
                new(3, 1),
                new(4, 1),
                new(5, 1),
            },
            143.7
        };

        yield return new object[]
        {
            new List<OrderedProductCreateDto>()
            {
                new(1, 5),
                new(2, 5),
                new(3, 5),
                new(4, 5),
                new(5, 5),
            },
            436.5
        };
    }



    private static IEnumerable<ProductType> GetTestProductTypes()
    {
        return new[]
        {
            new ProductType
            {
                Id = 1,
                Name = "PhotoBook",
                StackSize = 1,
                RequiredBinWidth = 19
            },
            new ProductType
            {
                Id = 2,
                Name = "Calendar",
                StackSize = 1,
                RequiredBinWidth = 10
            },
            new ProductType
            {
                Id = 3,
                Name = "Canvas",
                StackSize = 1,
                RequiredBinWidth = 16
            },
            new ProductType
            {
                Id = 4,
                Name = "Cards",
                StackSize = 1,
                RequiredBinWidth = 4.7
            },
            new ProductType
            {
                Id = 5,
                Name = "Mug",
                StackSize = 4,
                RequiredBinWidth = 94
            }
        }.ToList();
    }

    #endregion
}