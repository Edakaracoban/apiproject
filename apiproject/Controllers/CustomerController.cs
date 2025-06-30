using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiproject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace apiproject.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        public CustomerController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("GetCustomer")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }




        [HttpPost]
        [Route("AddCustomer")]
        public async Task<Customer> AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }




        [HttpPut]
        [Route("UpdateCustomer/ {id}")]
        public async Task<Customer> UpdateCustomer(Customer customer, int id)
        {
            _context.Entry(customer).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return customer;
        }



        [HttpDelete]
        [Route("DeleteCustomer/{id}")]
        public bool DeleteCustomer(int id)
        {
            bool a = false;
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {

                a = true;
                _context.Entry(customer).State = EntityState.Deleted;
                _context.SaveChangesAsync();
            }
            else
            {
                a = false;
            }
            return a;
        }

    }
}

