using Application.DataTransferObjects;
using Application.Features.Product.Queries;

namespace WebAPI.Controllers;

public class ProductController : ApiControllerBase
{
    [HttpGet]
    [Produces(typeof(IList<ProductDto>))]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetProductsQuery();
        return Ok(await Mediator.Send(query));
    }
}