using chuloon.Data;
using chuloon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Repos
{
    public class MenuItemRepo
    {

        private readonly ChuloonContext _ctx;

        public MenuItemRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public MenuItem Get(int id)
        {
            return _ctx.MenuItems.Find(id);
        }

        public IQueryable<MenuCategory> CategoryList()
        {
            IQueryable<MenuCategory> categs;
            categs = _ctx.MenuCategories.OrderBy(c => c.Seq);
            return categs;
        }

        public void Add(MenuItem item)
        {
            _ctx.MenuItems.Add(item);
            _ctx.SaveChanges();
        }

        public void Edit(MenuItem itemChanges)
        {
            var item = _ctx.MenuItems.Find(itemChanges.Id);
            item.Name = itemChanges.Name;
            item.MenuCategoryId = itemChanges.MenuCategoryId;
            item.Notes = itemChanges.Notes;
            _ctx.SaveChanges();
        }
    }
}
