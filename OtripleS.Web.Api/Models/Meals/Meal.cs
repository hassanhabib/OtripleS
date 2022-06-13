// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.Meals
{
    public class Meal
    {
        public Guid Id { get; set; }
        public enum Type { Breakfast, Lunch, Dinner};
        public double Price { get; set; }
        public Guid StudentId { get; set; }
        public enum Status { Ready, Pickedup, Delivered, Cancelled};
        public int Quantity { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }

    }
}
