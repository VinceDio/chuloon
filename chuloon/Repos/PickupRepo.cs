using chuloon.Data;
using chuloon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Repos
{
    public class PickupRepo
    {

        private readonly ChuloonContext _ctx;

        public PickupRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public Pickup Get(int id)
        {
            return _ctx.Pickup.Include(p => p.Restaurant)
                .Include(p => p.PickupUser)
                .Include(p => p.Orders).ThenInclude(o => o.User)
                .Include(p => p.Orders).ThenInclude(o => o.OrderItems).ThenInclude(i => i.MenuItem)
                .SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Pickup> PickupsByDate(DateTime date)
        {
            return _ctx.Pickup.Include(p => p.PickupUser)
                .Include(p => p.Restaurant)
                .Where(p => p.Date == date)
                .OrderBy(p => p.Restaurant.Name);
        }

        public Pickup Edit(Pickup changes)
        {
            var pickup = _ctx.Pickup.Find(changes.Id);
            if (pickup == null) return null;
            pickup.Note = changes.Note;
            pickup.PickupTime = changes.PickupTime;
            pickup.PickupUserId = changes.PickupUserId;
            _ctx.Update(pickup);
            var result = _ctx.SaveChanges();
            if (result == 1) return pickup;
            else return null;
        }

        public bool Delete(int pickupId)
        {
            var pickup = _ctx.Pickup.Find(pickupId);
            if (pickup == null) return false;
            //remove orders and items
            foreach (var order in pickup.Orders)
            {
                foreach (var item in order.OrderItems)
                {
                    _ctx.OrderItem.Remove(item);
                }
                _ctx.Order.Remove(order);
            }
            _ctx.Pickup.Remove(pickup);
            _ctx.SaveChanges();
            return true;
        }

        public List<OrderItem> PickupTotals(int pickupId)
        {
            var items = _ctx.OrderItem
                .Include(i => i.MenuItem)
                .Include(i => i.Order).ThenInclude(o => o.User)
                .Where(o => o.Order.PickupId == pickupId)
                .OrderBy(i => i.MenuItem.MenuCategory.Seq)
                .ThenBy(i => i.MenuItem.Name);
            return items.ToList();
        }
    }
}
