// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentContact> InsertStudentContactAsync(
           StudentContact studentContact);

        IQueryable<StudentContact> SelectAllStudentContacts();

        ValueTask<StudentContact> SelectStudentContactByIdAsync(
           Guid studentId,
           Guid contactId);

        ValueTask<StudentContact> UpdateStudentContactAsync(
           StudentContact studentContact);

        ValueTask<StudentContact> DeleteStudentContactAsync(
           StudentContact studentContact);
    }
}