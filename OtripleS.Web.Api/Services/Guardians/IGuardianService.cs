// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Guardian;

namespace OtripleS.Web.Api.Services.Guardians
{
    public interface IGuardianService
    {
        ValueTask<Guardian> RetrieveGuardianByIdAsync(Guid guardianId);
        ValueTask<Guardian> CreateGuardianAsync(Guardian guardian);
    }
}
