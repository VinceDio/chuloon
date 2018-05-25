using chuloon.Data;
using chuloon.Models;
using chuloon.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Repos
{
    public class MenuRepo
    {

        private readonly ChuloonContext _ctx;

        public MenuRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public MenuViewModel GetViewModel(int restaurantId, int? addToOrderId = null)
        {
            var vm = new MenuViewModel();
            vm.Restaurant = _ctx.Restaurants.Find(restaurantId);
            vm.AddToOrderId = addToOrderId;
            vm.Categories = new List<MenuViewModelCategory>();
            var items = _ctx.MenuItems.Include(i => i.Prices).Where(i => i.RestaurantId == restaurantId).OrderBy(i => i.MenuCategory.Seq);
            var categs = items.Select(i => i.MenuCategory).Distinct().OrderBy(c => c.Seq);
            foreach (MenuCategory categ in categs)
            {
                var vcateg = new MenuViewModelCategory();
                vcateg.Category = categ;
                vcateg.Items = new List<MenuViewModelItem>();
                foreach (MenuItem item in items.Where(i => i.MenuCategoryId == categ.Id).OrderBy(i => i.Name))
                {
                    var vitem = new MenuViewModelItem();
                    vitem.Item = item;
                    vitem.Prices = item.Prices.OrderBy(p => p.Price).ToList();
                    vcateg.Items.Add(vitem);
                }
                vm.Categories.Add(vcateg);
            }
            return vm;
        }

    }
}
