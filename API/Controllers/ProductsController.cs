using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductRepository repo) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand,
        string? type, string? sort)
        {
            var results = await repo.GetProductsAsync(brand, type, sort);
            return Ok(results);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetOneProduct(int id)
        {
            var results = await repo.GetProductByIdAsync(id);

            return results is null ? NotFound() : Ok(results);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repo.AddProduct(product);
            if (await repo.SaveChangesAsync())
            {
                return CreatedAtAction("GetOneProduct", new { id = product.ProductId }, product);
            }
            return BadRequest("Problem creating product");
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.ProductId != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            repo.UpdateProduct(product);
            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem updating the product");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repo.GetProductByIdAsync(id);
            if (product is null) return NotFound();
            repo.DeleteProduct(product);

            if (await repo.SaveChangesAsync())
            {
                return NoContent();
            }
            return BadRequest("Problem deleting the product");

        }
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
        {
            return Ok(await repo.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
        {
            return Ok(await repo.GetTypesAsync());
        }

        private bool ProductExists(int id)
        {
            return repo.ProductExists(id);
        }
    }
}
