using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using chuloon.Models;
using chuloon.Repos;
using Microsoft.AspNetCore.Authorization;

namespace chuloon.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly Repo _repo;

        public HomeController(Repo repo)
        {
            _repo = repo;
        }

        [AllowAnonymous]
        public IActionResult Index(DateTime? date)
        {
            if (date == null) date = DateTime.Today;
            ViewBag.Date = date;
            IEnumerable<Pickup> pickups = new List<Pickup>();
            if (User.Identity.IsAuthenticated) pickups = _repo.pickups.PickupsByDate((DateTime)date);
            return View(pickups);
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
