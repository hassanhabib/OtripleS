//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public interface ITeacherContactService
    {
        ValueTask<TeacherContact> AddTeacherContactAsync(TeacherContact teacherContact);
        IQueryable<TeacherContact> RetrieveAllTeacherContacts();
        ValueTask<TeacherContact> RetrieveTeacherContactByIdAsync(Guid teacherId, Guid contactId);
        ValueTask<TeacherContact> RemoveTeacherContactByIdAsync(Guid teacherId, Guid contactId);
    }
}