//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;

namespace OtripleS.Web.Api.Services.TeacherContacts
{
    public partial class TeacherContactService
    {
        private void ValidateTeacherContactOnAdd(TeacherContact teacherContact)
        {
            ValidateTeacherContactIsNull(teacherContact);
            ValidateTeacherContactRequiredFields(teacherContact);
        }

        private void ValidateTeacherContactIsNull(TeacherContact teacherContact)
        {
            if (teacherContact is null)
            {
                throw new NullTeacherContactException();
            }
        }

        private void ValidateTeacherContactRequiredFields(TeacherContact teacherContact)
        {
            switch (teacherContact)
            {
                case { } when IsInvalid(teacherContact.TeacherId):
                    throw new InvalidTeacherContactInputException(
                        parameterName: nameof(TeacherContact.TeacherId),
                        parameterValue: teacherContact.TeacherId);

                case { } when IsInvalid(teacherContact.ContactId):
                    throw new InvalidTeacherContactInputException(
                        parameterName: nameof(TeacherContact.ContactId),
                        parameterValue: teacherContact.ContactId);
            }
        }

        private void ValidateTeacherContactIdIsNull(Guid teacherId, Guid contactId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherContactInputException(
                    parameterName: nameof(TeacherContact.TeacherId),
                    parameterValue: teacherId);
            }

            if (contactId == default)
            {
                throw new InvalidTeacherContactInputException(
                    parameterName: nameof(TeacherContact.ContactId),
                    parameterValue: contactId);
            }
        }

        private static void ValidateStorageTeacherContact(
            TeacherContact storageTeacherContact,
            Guid teacherId, Guid contactId)
        {
            if (storageTeacherContact == null)
            {
                throw new NotFoundTeacherContactException(teacherId, contactId);
            }
        }

        private void ValidateStorageTeacherContacts(IQueryable<TeacherContact> storageTeacherContacts)
        {
            if (!storageTeacherContacts.Any())
            {
                this.loggingBroker.LogWarning("No teacherContacts found in storage.");
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
    }
}