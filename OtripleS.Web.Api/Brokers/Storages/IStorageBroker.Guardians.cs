// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Guardians;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
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
