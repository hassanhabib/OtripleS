// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Meals;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Meal> Meals { get; set; }

        public async ValueTask<Meal> InsertMealAsync(Meal meal)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Meal> mealEntityEntry = 
                await broker.Meals.AddAsync(entity: meal);

            await broker.SaveChangesAsync();

            return mealEntityEntry.Entity;
        }
    }
}
