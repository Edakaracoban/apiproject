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
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("GetProduct")]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }




        [HttpPost]
        [Route("AddProduct")]
        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }




        [HttpPut]
        [Route("UpdateProduct/ {id}")]
        public async Task<Product> UpdateProduct(Product product, int id)
        {
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return product;
        }



        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public bool DeleteProduct(int id)
        {
            bool a = false;
            var product = _context.Products.Find(id);
            if (product != null)
            {

                a = true;
                _context.Entry(product).State = EntityState.Deleted;
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

