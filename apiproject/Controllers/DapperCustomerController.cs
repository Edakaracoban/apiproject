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
    public class DapperCustomerController : ControllerBase
    {
        private readonly string _connectionString;

        public DapperCustomerController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
        }

        [HttpGet]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, FullName, Email FROM Customers";
            return await connection.QueryAsync<Customer>(sql);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomerById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "SELECT Id, FullName, Email FROM Customers WHERE Id = @Id";
            var customer = await connection.QueryFirstOrDefaultAsync<Customer>(sql, new { Id = id });

            if (customer == null)
                return NotFound();

            return customer;
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> AddCustomer(Customer customer)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "INSERT INTO Customers (FullName, Email) VALUES (@FullName, @Email); SELECT CAST(SCOPE_IDENTITY() as int)";
            var id = await connection.QuerySingleAsync<int>(sql, customer);
            customer.Id = id;

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
                return BadRequest();

            using var connection = new SqlConnection(_connectionString);
            var sql = "UPDATE Customers SET FullName = @FullName, Email = @Email WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, customer);

            if (result == 0)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var sql = "DELETE FROM Customers WHERE Id = @Id";
            var result = await connection.ExecuteAsync(sql, new { Id = id });

            if (result == 0)
                return NotFound();

            return NoContent();
        }
    }
}
