// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Meals;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker {
        ValueTask<Meal> InsertMealAsync(Meal meal);
        IQueryable<Meal> SelectAllMeals();
        ValueTask<Meal> UpdateMealAsync(Meal meal);
        ValueTask<Meal> DeleteMealAsync(Meal meal);
    }
}
