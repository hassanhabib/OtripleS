using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Guardian;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Guardian> InsertGuardianAsync(Guardian guardian);
        IQueryable<Guardian> SelectAllGuardians();
        ValueTask<Guardian> SelectGuardianByIdAsync(Guid guardianId);
        ValueTask<Guardian> UpdateGuardianAsync(Guardian guardian);
        ValueTask<Guardian> DeleteGuardianAsync(Guardian guardian);
    }
}
