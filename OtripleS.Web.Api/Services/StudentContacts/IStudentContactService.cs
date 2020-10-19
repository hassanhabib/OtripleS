//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Services.StudentContacts
{
    public interface IStudentContactService
    {
        ValueTask<StudentContact> AddStudentContactAsync(StudentContact studentContact);
        IQueryable<StudentContact> RetrieveAllStudentContacts();
        ValueTask<StudentContact> RetrieveStudentContactByIdAsync(Guid studentId, Guid contactId);
        ValueTask<StudentContact> RemoveStudentContactByIdAsync(Guid studentId, Guid contactId);
    }
}