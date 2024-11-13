using Microsoft.AspNetCore.Mvc;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Api.Controllers;

[ApiController]
public class ProductsController(IProductsService productsService) : ControllerBase
{
    [HttpGet("[controller]")]
    public async Task<ActionResult<IEnumerable<Product>>> Products()
        => Ok(await productsService.GetAllProducts());

    [HttpGet("[controller]/{name}")]
    public async Task<ActionResult<Product>> Product(string name)
    {
        var product = await productsService.GetProductByName(name);
        if (product is null)
            return NotFound();
        return Ok(product);
    }

    [HttpGet("[controller]/search")]
    public async Task<ActionResult<IEnumerable<Product>>> ProductsSearch([FromQuery] string query)
        => Ok(await productsService.GetProductByLikeName(query));
}
