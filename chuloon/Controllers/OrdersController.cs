using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using chuloon.Data;
using chuloon.Models;
using Microsoft.AspNetCore.Identity;
using chuloon.Repos;
using System.Text;

namespace chuloon.Controllers
{
    public class OrdersController : Controller
    {
        
        private readonly ChuloonContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Repo _repo;


        public OrdersController(ChuloonContext context, UserManager<ApplicationUser> userManager, Repo repo)
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var chuloonContext = _context.Order.Include(o => o.PickUp).Include(o => o.User);
            return View(await chuloonContext.ToListAsync());
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id, int? pickupId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _repo.orders.Get((int)id);

            var userid = _userManager.GetUserId(HttpContext.User);
            //if order doesnt exist yet, create temporary empty order
            if (id == 0 && pickupId != null)
            {
                order = _repo.orders.Add(userid, (int)pickupId);
                return RedirectToAction("Edit", new { id = order.Id });
            }

            if (order == null)
            {
                return NotFound();
            }

            if (userid != order.UserId && userid != order.PickUp.PickupUserId)
            {

                return RedirectToAction("Details", "Pickups", new { id = order.PickupId });
            }
            byte[] bytes = Encoding.ASCII.GetBytes(Request.Path + Request.QueryString);
            HttpContext.Session.Set("MenuReturnTo", bytes);
            return View(order);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string action, Order order)
        {
            if (action == "Cancel Order") _repo.orders.Cancel(order.Id);
            else _repo.orders.Update(order);
            return RedirectToAction("Details", "Pickups", new { id = order.PickupId });
        }

        
    }
}
