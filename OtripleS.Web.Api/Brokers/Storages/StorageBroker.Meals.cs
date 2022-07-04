// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Meals;
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

        public IQueryable<Meal> SelectAllMeals() => this.Meals;

        public async ValueTask<Meal> SelectMealByIdAsync(Guid mealId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Meals.FindAsync(mealId);
        }

        public async ValueTask<Meal> UpdateMealAsync(Meal meal)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Meal> mealEntityEntry =
                broker.Meals.Update(entity: meal);

            await broker.SaveChangesAsync();

            return mealEntityEntry.Entity;
        }

        public async ValueTask<Meal> DeleteMealAsync(Meal meal)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<Meal> mealEntityEntry = broker.Meals.Remove(entity: meal);

            await broker.SaveChangesAsync();

            return mealEntityEntry.Entity;
        }
    }
}
