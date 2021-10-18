// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Services.Foundations.Registrations
{
    public interface IRegistrationService
    {
        ValueTask<Registration> AddRegistrationAsync(Registration registration);
        IQueryable<Registration> RetrieveAllRegistrations();
        ValueTask<Registration> RetrieveRegistrationByIdAsync(Guid registrationId);
        ValueTask<Registration> ModifyRegistrationAsync(Registration registration);
        ValueTask<Registration> RemoveRegistrationByIdAsync(Guid registrationId);
    }
}
