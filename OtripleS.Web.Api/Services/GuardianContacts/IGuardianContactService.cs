//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.GuardianContacts;

namespace OtripleS.Web.Api.Services.GuardianContacts
{
    public interface IGuardianContactService
    {
        ValueTask<GuardianContact> AddGuardianContactAsync(GuardianContact guardianContact);
        ValueTask<GuardianContact> RemoveGuardianContactByIdAsync(Guid guardianId, Guid contactId);
        IQueryable<GuardianContact> RetrieveAllGuardianContacts();
        ValueTask<GuardianContact> RetrieveGuardianContactByIdAsync(Guid guardianId, Guid contactId);
    }
}