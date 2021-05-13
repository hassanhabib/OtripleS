// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Registrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<Registration> InsertRegistrationAsync(Registration registration);
        public IQueryable<Registration> SelectAllRegistrations();
        public ValueTask<Registration> SelectRegistrationByIdAsync(Guid registrationId);
        public ValueTask<Registration> UpdateRegistrationAsync(Registration registration);
        public ValueTask<Registration> DeleteRegistrationAsync(Registration registration);
    }
}
