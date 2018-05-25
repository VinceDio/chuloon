using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int PickupId { get; set; }
        [MaxLength(450)]
        public string UserId { get; set; }
        public decimal Total { get; set; }
        public bool Paid { get; set; }

        public Pickup PickUp { get; set; }

        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }
    }
}
