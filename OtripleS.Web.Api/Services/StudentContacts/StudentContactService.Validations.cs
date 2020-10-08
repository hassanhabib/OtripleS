//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Linq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;

namespace OtripleS.Web.Api.Services.StudentContacts
{
	public partial class StudentContactService
    {
        private void ValidateStudentContactOnCreate(StudentContact studentContact)
        {
            ValidateStudentContactIsNull(studentContact);
        }

        private void ValidateStudentContactIsNull(StudentContact studentContact)
        {
            if (studentContact is null)
            {
                throw new NullStudentContactException();
            }
        }

        private void ValidateStorageStudentContacts(IQueryable<StudentContact> storageStudentContacts)
        {
            if (!storageStudentContacts.Any())
            {
                this.loggingBroker.LogWarning("No studentContacts found in storage.");
            }
        }
    }
}
