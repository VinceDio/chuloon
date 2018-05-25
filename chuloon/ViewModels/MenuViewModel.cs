using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chuloon.Models;

namespace chuloon.ViewModels
{
    public class MenuViewModel
    {
        public Restaurant Restaurant { get; set; }
        public int? AddToOrderId { get; set; }
        public List<MenuViewModelCategory> Categories { get; set; }
    }

    public class MenuViewModelCategory
    {
        public MenuCategory Category { get; set; }
        public List<MenuViewModelItem> Items { get; set; }
    }

    public class MenuViewModelItem
    {
        public MenuItem Item { get; set; }
        public List<MenuPrice> Prices { get; set; }
    }
}
