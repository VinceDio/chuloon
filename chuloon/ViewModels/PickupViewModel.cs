using chuloon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.ViewModels
{
    public class PickupViewModel
    {
        public Pickup Pickup { get; set; }

        public List<Order> Orders { get; set; }

        public List<OrderItem> TotalItems { get; set; }

    }
}
