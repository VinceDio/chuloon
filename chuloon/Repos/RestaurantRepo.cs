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
    public class RestaurantRepo
    {

        private readonly ChuloonContext _ctx;

        public RestaurantRepo(ChuloonContext ctx)
        {
            _ctx = ctx;
        }

        public Restaurant Get(int id)
        {
            return _ctx.Restaurants.Find(id);
        }


    }
}
