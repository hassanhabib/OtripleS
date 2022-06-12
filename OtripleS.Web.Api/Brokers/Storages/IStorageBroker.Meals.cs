using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Meals;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public DbSet<Meal> Meals { get; set; }
        
    }
}
