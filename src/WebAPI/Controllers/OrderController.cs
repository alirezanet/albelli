using Application.DataTransferObjects;

namespace WebAPI.Controllers;

public class OrderController : ApiControllerBase
{
    [HttpGet("{id:int}")]
    [Produces(typeof(OrderDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = new GetOrderByIdQuery(id);
        var result = await Mediator.Send(order);
        return result != null ? Ok(result) : NotFound();
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder([FromBody] IList<OrderedProductCreateDto> products)
    {
        var command = new CreateOrderCommand(products);
        var result = await Mediator.Send(command);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}