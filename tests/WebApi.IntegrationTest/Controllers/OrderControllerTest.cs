using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Application.DataTransferObjects;
using AutoBogus;
using FluentAssertions;
using Newtonsoft.Json;
using WebApi.IntegrationTest;
using Xunit;

namespace WebAPI.IntegrationTest.Controllers;

public class OrderControllerTest : IntegrationTestBase
{

    [Fact]
    public async Task CreateOrder_ShouldSaveTheOrderAndReturnFriendlyOrderInfo()
    {
        // Arrange
        var payloadData = new AutoFaker<OrderedProductCreateDto>()
            .RuleFor(q => q.ProductId, f => f.Random.Int(1, 5))
            .Generate(2);

        var jsonPayload = JsonConvert.SerializeObject(payloadData);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        // Act
        var response = await TestClient.PostAsync("/api/order/", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var str = await response.Content.ReadAsStringAsync();
        str.Should().StartWith("OrderID: 1, please prepare a bin with minimum width of");
    }

    [Fact]
    public async Task GetById_WhenOrderNotExist_ShouldReturnNotFound()
    {
        // Act
        var response = await TestClient.GetAsync("/api/order/10");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

}