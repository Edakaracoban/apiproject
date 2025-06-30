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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        [Route("GetCategory")]
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }




        [HttpPost]
        [Route("AddCategory")]
        public async Task<Category> AddCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }




        [HttpPut]
        [Route("UpdateCategory/ {id}")]
        public async Task<Category> UpdateCategory(Category category, int id)
        {
            _context.Entry(category).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return category;
        }



        [HttpDelete]
        [Route("DeleteCategory/{id}")]
        public bool DeleteCategory(int id)
        {
            bool a = false;
            var category = _context.Categories.Find(id);
            if (category != null)
            {

                a = true;
                _context.Entry(category).State = EntityState.Deleted;
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

