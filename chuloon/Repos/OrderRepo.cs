using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chuloon.Models;
using chuloon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace chuloon.Repos
{
    public class OrderRepo
    {
        private readonly ChuloonContext _ctx;

        public OrderRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public Order Get(int id)
        {
            return _ctx.Order
                .Include(o => o.PickUp).ThenInclude(p => p.Restaurant)
                .Include(o => o.PickUp).ThenInclude(p => p.PickupUser)
                .Include(o => o.User)
                .Include(o => o.OrderItems).ThenInclude(i => i.MenuItem)
                .Where(o => o.Id == id)
                .FirstOrDefault();
        }

        public List<Order> PickupOrders(int pickupId, string currentUser)
        {
            var orders = _ctx.Order
                .Include(o => o.OrderItems).ThenInclude(i => i.MenuItem)
                .Where(o => o.PickupId == pickupId)
                .OrderByDescending(o => o.UserId == currentUser ? 1 : 0);
            return orders.ToList();
        }

        public Order Add(string userId, int pickupId)
        {
            var order = new Order
            {
                PickupId = pickupId,
                UserId = userId,
                Paid = false,
                Total = 0
            };
            _ctx.Order.Add(order);
            _ctx.SaveChanges();
            return Get(order.Id);
        }

        public Order Update(Order changes)
        {
            var order = _ctx.Order.Find(changes.Id);
            if (order == null) return null;
            order.Paid = changes.Paid;
            _ctx.Update(order);
            _ctx.SaveChanges();
            UpdateItems(changes.OrderItems);
            UpdateTotal(order.Id);
            return order;
        }

        private void UpdateItems(List<OrderItem> items)
        {
            foreach(var changes in items)
            {
                var item = _ctx.OrderItem.Find(changes.Id);
                if (item != null)
                {
                    item.Qty = changes.Qty;
                    item.Note = changes.Note;
                    _ctx.OrderItem.Update(item);
                }
            }
            _ctx.SaveChanges();
        }

        public OrderItem AddItem(int orderId, int priceId)
        {
            var order = _ctx.Order.Find(orderId);
            if (order == null) return null;
            var price = _ctx.MenuPrices.Find(priceId);
            if (price == null) return null;
            var item = new OrderItem
            {
                OrderId = order.Id,
                MenuItemId = price.MenuItemId,
                Qty = 1,
                PriceName = price.Name,
                Price = price.Price,
            };
            _ctx.OrderItem.Add(item);
            _ctx.SaveChanges();
            UpdateTotal(orderId);
            return item;
        }

        public OrderItem RemoveItem(int itemId)
        {
            var item = _ctx.OrderItem.Find(itemId);
            _ctx.OrderItem.Remove(item);
            _ctx.SaveChanges();
            UpdateTotal(item.OrderId);
            return item;
        }

        public decimal UpdateTotal(int orderId)
        {
            decimal total = _ctx.OrderItem.Where(i => i.OrderId == orderId).Sum(i => i.Qty * i.Price);
            var order = _ctx.Order.Find(orderId);
            order.Total = total;
            _ctx.Update(order);
            _ctx.SaveChanges();
            return total;
        }

        public void Cancel(int orderId)
        {
            var order = _ctx.Order.Find(orderId);
            if (order == null) return;
            _ctx.OrderItem.RemoveRange(_ctx.OrderItem.Where(i => i.OrderId == orderId));
            _ctx.Order.Remove(order);
            _ctx.SaveChanges();
        }
    }
}
