using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using chuloon.Data;
using chuloon.Models;
using chuloon.Repos;

namespace chuloon.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly ChuloonContext _context;
        private readonly Repo _repo;

        public OrderItemsController(ChuloonContext context, Repo repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: OrderItems
        public async Task<IActionResult> Index()
        {
            var chuloonContext = _context.OrderItem.Include(o => o.MenuItem).Include(o => o.Order);
            return View(await chuloonContext.ToListAsync());
        }


        public async Task<IActionResult> Create(int orderId, int priceId)
        {
            _repo.orders.AddItem(orderId, priceId);
            return RedirectToAction("Edit", "Orders", new { id = orderId });
        }

        // GET: OrderItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem.SingleOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", orderItem.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // POST: OrderItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,MenuItemId,Qty,PriceName,Price,Note")] OrderItem orderItem)
        {
            if (id != orderItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderItemExists(orderItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MenuItemId"] = new SelectList(_context.MenuItems, "Id", "Name", orderItem.MenuItemId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", orderItem.OrderId);
            return View(orderItem);
        }

        // GET: OrderItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderItem = await _context.OrderItem
                .Include(o => o.MenuItem)
                .Include(o => o.Order)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (orderItem == null)
            {
                return NotFound();
            }

            return View(orderItem);
        }

        // POST: OrderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = _repo.orders.RemoveItem(id);
            return RedirectToAction("Edit", "Orders", new { id = item.OrderId });
        }

        private bool OrderItemExists(int id)
        {
            return _context.OrderItem.Any(e => e.Id == id);
        }
    }
}
