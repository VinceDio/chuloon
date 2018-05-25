using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using chuloon.ViewModels;
using chuloon.Repos;
using Microsoft.AspNetCore.Mvc.Rendering;
using chuloon.Models;
using System.Text;

namespace chuloon.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly Repo _repo;

        public MenuItemsController(Repo repo)
        {
            _repo = repo;
        }

        public IActionResult Create(int? restaurantId)
        {
            if (restaurantId == null) return NotFound();
            var vm = new MenuItemViewModel();
            vm.Restaurant = _repo.restaurants.Get((int)restaurantId);
            LoadCategorySelectList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MenuItemViewModel vm)
        {
            vm.MenuItem.RestaurantId = vm.Restaurant.Id;
            _repo.items.Add(vm.MenuItem);
            return RedirectToAction("Edit", new { id = vm.MenuItem.Id });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var vm = new MenuItemViewModel();
            vm.MenuItem = _repo.items.Get((int)id);
            vm.Restaurant = _repo.restaurants.Get(vm.MenuItem.RestaurantId);
            vm.Prices = _repo.prices.ItemPrices(vm.MenuItem.Id).ToList();
            LoadCategorySelectList();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MenuItemViewModel vm)
        {
            var item = _repo.items.Get(vm.MenuItem.Id);
            if (item == null) return NotFound();

            _repo.items.Edit(vm.MenuItem);
            for (int i = 0; i < vm.Prices.Count; i++)
            {
                var p = vm.Prices[i];
                if (p.Price > 0)
                {
                    if (p.Id == 0)
                    {
                        p.MenuItemId = vm.MenuItem.Id;
                        _repo.prices.Add(p);
                    }
                    else
                    {
                        if (vm.DeletePrice[i]) _repo.prices.Delete(p.Id);
                        _repo.prices.Edit(p);
                    }
                }
            }
            byte[] bytes;
            HttpContext.Session.TryGetValue("MenuReturnTo", out bytes);
            if (bytes == null) return RedirectToAction("Index", "Menu", new { restaurantId = item.RestaurantId });
            else
            {
                string returnTo = Encoding.ASCII.GetString(bytes);
                return Redirect(returnTo);
            }
            
        }


        private void LoadCategorySelectList()
        {
            var categs = new SelectList(_repo.items.CategoryList(), "Id", "Name");
            ViewBag.MenuCategoryId = categs;
        }
    }
}