// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.GuardianContacts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<GuardianContact> InsertGuardianContactAsync(
           GuardianContact guardianContact);

        IQueryable<GuardianContact> SelectAllGuardianContacts();

        ValueTask<GuardianContact> SelectGuardianContactByIdAsync(
           Guid guardianId,
           Guid contactId);

        ValueTask<GuardianContact> UpdateGuardianContactAsync(
           GuardianContact guardianContact);

        ValueTask<GuardianContact> DeleteGuardianContactAsync(
           GuardianContact guardianContact);
    }
}