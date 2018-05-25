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
    public class MenuPriceRepo
    {

        private readonly ChuloonContext _ctx;

        public MenuPriceRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public MenuPrice Get(int id)
        {
            return _ctx.MenuPrices.Find(id);
        }

        public IQueryable<MenuPrice> ItemPrices(int itemId)
        {
            return _ctx.MenuPrices.Where(p => p.MenuItemId == itemId).OrderBy(p => p.Price).ThenBy(p => p.Name);
        }

        public void Add(MenuPrice price)
        {
            _ctx.MenuPrices.Add(price);
            _ctx.SaveChanges();
        }

        public void Edit(MenuPrice priceChanges)
        {
            var price = _ctx.MenuPrices.Find(priceChanges.Id);
            if (price == null) return;
            price.Name = priceChanges.Name;
            price.Price = priceChanges.Price;
            price.Note = priceChanges.Note;
            _ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            var price = _ctx.MenuPrices.Find(id);
            _ctx.MenuPrices.Remove(price);
        }
    }
}
