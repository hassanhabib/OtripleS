// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<StudentContact> InsertStudentContactAsync(
            StudentContact studentContact);

        public IQueryable<StudentContact> SelectAllStudentContacts();

        public ValueTask<StudentContact> SelectStudentContactByIdAsync(
            Guid studentId,
            Guid contactId);

        public ValueTask<StudentContact> UpdateStudentContactAsync(
            StudentContact studentContact);

        public ValueTask<StudentContact> DeleteStudentContactAsync(
            StudentContact studentContact);
    }
}
