using System.Linq;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Fees
{
    public interface IFeeService
    {
        IQueryable<Fee> RetrieveAllFees();
    }
}
