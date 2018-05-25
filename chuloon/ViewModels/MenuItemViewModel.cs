using chuloon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.ViewModels
{
    public class MenuItemViewModel
    {
        public Restaurant Restaurant { get; set; }
        public MenuItem MenuItem { get; set; }
        public List<MenuPrice> Prices { get; set; }
        public List<bool> DeletePrice { get; set; }
    }
}
