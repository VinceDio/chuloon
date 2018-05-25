using chuloon.Data;
using chuloon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chuloon.Repos
{
    public class Repo
    {
        private readonly ChuloonContext _ctx;

        public RestaurantRepo restaurants;
        public MenuItemRepo items;
        public MenuPriceRepo prices;
        public MenuRepo menu;
        public PickupRepo pickups;
        public OrderRepo orders;

        public Repo(ChuloonContext ctx)
        {
            _ctx = ctx;
            restaurants = new RestaurantRepo(_ctx);
            items = new MenuItemRepo(_ctx);
            prices = new MenuPriceRepo(_ctx);
            menu = new MenuRepo(_ctx);
            pickups = new PickupRepo(_ctx);
            orders = new OrderRepo(_ctx);
        }



    }
}
