using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using chuloon.Data;
using chuloon.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using chuloon.Repos;
using chuloon.ViewModels;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace chuloon.Controllers
{
    public class PickupsController : Controller
    {
        private readonly ChuloonContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Repo _repo;
        private readonly SlackController _slackController;

        public PickupsController(ChuloonContext context, UserManager<ApplicationUser> userManager, Repo repo)
        {
            _context = context;
            _userManager = userManager;
            _repo = repo;
            _slackController = new SlackController();
        }

        // GET: Pickups
        public async Task<IActionResult> Index(DateTime? date)
        {
            if (date == null) date = DateTime.Today;
            ViewBag.Date = date;
            var chuloonContext = _context.Pickup.Include(p => p.PickupUser).Where(p => p.Date == date);
            return View(await chuloonContext.ToListAsync());
        }

        // GET: Pickups/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vm = new PickupViewModel();
            vm.Pickup = _repo.pickups.Get((int)id);
            if (vm.Pickup == null)
            {
                return NotFound();
            }
            var userid = _userManager.GetUserId(HttpContext.User);
            vm.Orders = _repo.orders.PickupOrders(vm.Pickup.Id, userid);
            vm.TotalItems = _repo.pickups.PickupTotals(vm.Pickup.Id);
            //if current user not yet on pickup, add placeholder in orders for them
            if (vm.Orders.Exists(o => o.UserId == userid) == false)
            {
                vm.Orders.Insert(0, new Order {
                    PickupId = vm.Pickup.Id,
                    UserId = userid,
                    User = _context.Users.Find(userid)
                });
            }
            return View(vm);
        }

        // GET: Pickups/Create
        public IActionResult Create(int restaurantId)
        {

            var pickup = new Pickup();
            pickup.Date = DateTime.Today;
            var rest = _repo.restaurants.Get(restaurantId);
            if (rest == null) return RedirectToAction("Index", "Home");
            pickup.RestaurantId = rest.Id;
            pickup.Restaurant = rest;
            pickup.PickupTime = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-dd 12:00"));
            var userid = _userManager.GetUserId(User);
            LoadPickUpUserList(userid);
            return View(pickup);
        }

        // POST: Pickups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pickup newpickup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newpickup);
                await _context.SaveChangesAsync();
                if (Request.Form["sendSlack"] == "on")
                {
                    var pickup = _repo.pickups.Get(newpickup.Id);
                    var msg = string.Concat(pickup.PickupUser.NicknameOrUser, " is heading to ",
                        pickup.Restaurant.Name, " at ", pickup.PickupTime.ToString("HH:mm tt"), ". Get your orders in!");
                    var success = await PostToSlack(newpickup.Id, "New Pickup", msg);
                }
                return RedirectToAction(nameof(Details), new { id = newpickup.Id });
            }
            LoadPickUpUserList(newpickup.PickupUserId);
            return View(newpickup);
        }

        private async Task<bool> PostToSlack(int pickupId, string msgTitle, string msgText)
        {
            //add link to pickup to end of message
            msgText += Environment.NewLine + Url.Action("Details", "Pickups", new { id = pickupId }, Request.Scheme);
            await _slackController.Send(msgTitle, msgText);
            return true;
        }

        // GET: Pickups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pickup = await _context.Pickup.SingleOrDefaultAsync(m => m.Id == id);
            if (pickup == null)
            {
                return NotFound();
            }
            LoadPickUpUserList(pickup.PickupUserId);
            return View(pickup);
        }

        // POST: Pickups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,RestaurantId,PickupUserId,PickupTime,Note")] Pickup pickup)
        {
            if (id != pickup.Id)
            {
                return NotFound();
            }
            if (_repo.pickups.Edit(pickup) != null)
            {
                return RedirectToAction("Details", "Pickups", new { id = pickup.Id });
            }
            else
            {
                LoadPickUpUserList(pickup.PickupUserId);
                return View(pickup);
            }
        }

        // GET: Pickups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pickup = await _context.Pickup
                .Include(p => p.PickupUser)
                .Include(p => p.Restaurant)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pickup == null)
            {
                return NotFound();
            }

            return View(pickup);
        }

        // POST: Pickups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var pickup = _repo.pickups.Get(id);
            if (pickup == null) return RedirectToAction("Index", "Home");

            _repo.pickups.Delete(id);
            return RedirectToAction("Index", "Home", new { date = pickup.Date.ToString("yyyy-MM-dd") });
        }

        private bool PickupExists(int id)
        {
            return _context.Pickup.Any(e => e.Id == id);
        }

        private void LoadPickUpUserList(string userId)
        {
            ViewData["PickupUserId"] = new SelectList(_context.Users, "Id", "NicknameOrUser", userId);
        }
    }
}
