// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Guardians;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.Guardians
{
    public interface IGuardianService
    {
        ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId);
        IQueryable<Guardian> RetrieveAllGuardians();
        ValueTask<Guardian> CreateGuardianAsync(Guardian guardian);
        ValueTask<Guardian> RemoveGuardianByIdAsync(Guid guardianId);
        ValueTask<Guardian> ModifyGuardianAsync(Guardian guardian);
    }
}
