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
    public class DapperCategoryController : ControllerBase
    {
        private readonly string _connectionString;

        public DapperCategoryController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            Console.WriteLine($"CONNECTION STRING: {_connectionString}");
        }

        [HttpGet]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CategoryName FROM Categories";
            return await connection.QueryAsync<Category>(sql);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, CategoryName FROM Categories WHERE Id = @Id";
            var category = await connection.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });

            if (category == null)
                return NotFound();

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(Category category)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName); SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = await connection.QuerySingleAsync<int>(sql, category);
            category.Id = id;

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.Id)
                return BadRequest();

            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Categories SET CategoryName = @CategoryName WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, category);

            if (result == 0)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Categories WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });

            if (result == 0)
                return NotFound();

            return NoContent();
        }
    }
}
