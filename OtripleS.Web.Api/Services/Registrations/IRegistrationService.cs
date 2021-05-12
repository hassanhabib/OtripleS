// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Services.Registrations
{
    public interface IRegistrationService
    {
        ValueTask<Registration> RetrieveRegistrationByIdAsync(Guid registrationId);
    }
}
