﻿using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Products
    {
        [Key]
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
    }
}
