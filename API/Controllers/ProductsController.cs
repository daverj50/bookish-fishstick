using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(StoreContext db) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var results = await db.Products.ToListAsync();
            return Ok(results);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetOneProduct(int id)
        {
            var results = await db.Products.FindAsync(id);

            return results is null ? NotFound() : Ok(results);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            db.Products.Add(product);
            await db.SaveChangesAsync();
            return product;
        }
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.ProductId != id || !ProductExists(id))
                return BadRequest("Cannot update this product");

            db.Entry(product).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var results = await db.Products.FindAsync(id);
            if (results is null) return NotFound();
            db.Products.Remove(results);
            await db.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return db.Products.Any(x => x.ProductId == id);
        }
    }
}
