using System;
using System.ComponentModel.DataAnnotations;

namespace apiproject.Models
{
	public class Customer
	{
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}

