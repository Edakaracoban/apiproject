using System;
using System.ComponentModel.DataAnnotations;

namespace apiproject.Models
{
	public class Category
	{
        [Key]
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}

