// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Fees
{
    public interface IFeeService
    {
        IQueryable<Fee> RetrieveAllFees();
    }
}
