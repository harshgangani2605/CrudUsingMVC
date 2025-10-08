﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp1.Models
{
    public class Item
    {
        public int Id { get; set; }                     // Primary key
        public string Name { get; set; } = null!;
        public double Price { get; set; }

        public int? SerialNumberId { get; set; }
        public SerialNumber? SerialNumber { get; set; }
        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public List<ItemClient>? ItemClients { get; set; }
    }
}
