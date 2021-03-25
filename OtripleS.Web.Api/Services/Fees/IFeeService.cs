// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

<<<<<<< HEAD
using System.Threading.Tasks;
=======
using System.Linq;
>>>>>>> origin/master
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Fees
{
    public interface IFeeService
    {
<<<<<<< HEAD
        ValueTask<Fee> AddFeeAsync(Fee fee);
=======
        IQueryable<Fee> RetrieveAllFees();
>>>>>>> origin/master
    }
}
