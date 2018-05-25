using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using chuloon.Repos;
using Microsoft.AspNetCore.Mvc;

namespace chuloon.Controllers
{
    public class MenuController : Controller
    {
        private readonly Repo _repo;

        public MenuController(Repo repo)
        {
            _repo = repo;
        }

        public IActionResult Index(int restaurantId)
        {
            var r = _repo.restaurants.Get(restaurantId);
            if (r == null) return NotFound();

            byte[] bytes = Encoding.ASCII.GetBytes(Request.Path + Request.QueryString);
            HttpContext.Session.Set("MenuReturnTo", bytes);
            return View(r);
        }
    }
}