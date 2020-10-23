// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<GuardianContact> InsertGuardianContactAsync(
            GuardianContact guardianContact);

        public IQueryable<GuardianContact> SelectAllGuardianContacts();

        public ValueTask<GuardianContact> SelectGuardianContactByIdAsync(
            Guid guardianId,
            Guid contactId);

        public ValueTask<GuardianContact> UpdateGuardianContactAsync(
            GuardianContact guardianContact);

        public ValueTask<GuardianContact> DeleteGuardianContactAsync(
            GuardianContact guardianContact);
    }
}
