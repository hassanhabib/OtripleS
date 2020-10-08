//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Services.StudentContacts
{
    public interface IStudentContactService
    {
        IQueryable<StudentContact> RetrieveAllStudentContacts();
    }
}
