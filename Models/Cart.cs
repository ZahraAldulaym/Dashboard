﻿using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public string IdCustomer { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public double Total { get; set; }
        public int Qty { get; set; }
        public string Color { get; set; }
        public double Tax { get; set; }
    }
}
