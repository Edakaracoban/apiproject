using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using apiproject.Models;

namespace apiproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DapperProductController : ControllerBase
    {
        private readonly string _connectionString;
        //Dapper sorguları SQL cümlesiyle ve parametrelerle çalışır.
        //Dapper için SqlConnection kullan, connection string’i IConfiguration’dan al.
        //Controllers klasörüne yeni bir controller ekle. Bu EF ile alakalı AppDbContext kullanmaz. Direkt SqlConnection ve Dapper ile sorgu yapar:
        public DapperProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            Console.WriteLine($"CONNECTION STRING: {_connectionString}");
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> GetProducts() //Read //Listeleme
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "SELECT Id, Name, Price, Feature FROM Products";
            var products = await connection.QueryAsync<Product>(sql);
            return products;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product) // Create ekleme
        {
            using var connection = new SqlConnection(_connectionString);
            string sql = "INSERT INTO Products (Name, Price, Feature) VALUES (@Name, @Price, @Feature); SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = await connection.QuerySingleAsync<int>(sql, product);
            product.Id = id;
            return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
        }

        // GET: api/DapperProduct/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id) // Veri çekme idye göre
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, Name, Price, Feature FROM Products WHERE Id = @Id";
            var product = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });

            if (product == null)
                return NotFound();

            return product;
        }

        // PUT: api/DapperProduct/5 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product) // update // güncelleme
        {
            if (id != product.Id)
                return BadRequest();

            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Products SET Name = @Name, Price = @Price, Feature = @Feature WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, product);

            if (affectedRows == 0)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/DapperProduct/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) //delete //silme
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Products WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });

            if (affectedRows == 0)
                return NotFound();

            return NoContent();
        }
    }
}
