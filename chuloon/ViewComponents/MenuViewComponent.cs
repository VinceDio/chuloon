using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chuloon.Repos;
using chuloon.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace chuloon.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly Repo _repo;
        public MenuViewComponent(Repo repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(int restaurantId, int? addToOrderId)
        {
            var vm = _repo.menu.GetViewModel(restaurantId, addToOrderId);
            return View(vm);
        }

    }
}