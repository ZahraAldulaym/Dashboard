﻿using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set;}
        public string Email { get; set;}
        public int Phone { get; set;}
        public string Address { get; set;}

    }
}
