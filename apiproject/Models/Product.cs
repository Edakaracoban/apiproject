using System;
using System.ComponentModel.DataAnnotations;

namespace apiproject.Models
{
	public class Product
	{
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Feature { get; set; }
    }
}

