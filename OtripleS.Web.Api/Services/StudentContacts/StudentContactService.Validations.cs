//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
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
            ValidateStudentContactRequiredFields(studentContact);
        }

        private void ValidateStudentContactIsNull(StudentContact studentContact)
        {
            if (studentContact is null)
            {
                throw new NullStudentContactException();
            }
        }

        private void ValidateStudentContactRequiredFields(StudentContact studentContact)
        {
            switch (studentContact)
            {
                case { } when IsInvalid(studentContact.StudentId):
                    throw new InvalidStudentContactInputException(
                        parameterName: nameof(StudentContact.StudentId),
                        parameterValue: studentContact.StudentId);

                case { } when IsInvalid(studentContact.ContactId):
                    throw new InvalidStudentContactInputException(
                        parameterName: nameof(StudentContact.ContactId),
                        parameterValue: studentContact.ContactId);
            }
        }

        private void ValidateStorageStudentContacts(IQueryable<StudentContact> storageStudentContacts)
        {
            if (!storageStudentContacts.Any())
            {
                this.loggingBroker.LogWarning("No studentContacts found in storage.");
            }
        }

        private void ValidateStudentContactIdIsNull(Guid studentId, Guid contactId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentContactInputException(
                    parameterName: nameof(StudentContact.StudentId),
                    parameterValue: studentId);
            }

            if (contactId == default)
            {
                throw new InvalidStudentContactInputException(
                    parameterName: nameof(StudentContact.ContactId),
                    parameterValue: contactId);
            }
        }

        private static void ValidateStorageStudentContact(
            StudentContact storageStudentContact,
            Guid studentId, Guid contactId)
        {
            if (storageStudentContact == null)
            {
                throw new NotFoundStudentContactException(studentId, contactId);
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
    }
}