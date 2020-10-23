// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Guardians;

namespace OtripleS.Web.Api.Services.Guardians
{
    public interface IGuardianService
    {
        ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId);
        IQueryable<Guardian> RetrieveAllGuardians();
        ValueTask<Guardian> CreateGuardianAsync(Guardian guardian);
        ValueTask<Guardian> DeleteGuardianByIdAsync(Guid guardianId);
        ValueTask<Guardian> ModifyGuardianAsync(Guardian guardian);
    }
}
