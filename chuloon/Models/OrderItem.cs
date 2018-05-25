using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int MenuItemId { get; set; }

        public int Qty { get; set; }

        public string PriceName { get; set; }

        public decimal Price { get; set; }

        public string Note { get; set; }

        [Display(Name = "Item")]
        public MenuItem MenuItem { get; set; }

        public Order Order { get; set; }

    }
}
