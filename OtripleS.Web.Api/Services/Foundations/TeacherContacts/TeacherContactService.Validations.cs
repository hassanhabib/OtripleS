// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.TeacherContacts
{
    public partial class TeacherContactService
    {
        private static void ValidateTeacherContactOnAdd(TeacherContact teacherContact)
        {
            ValidateTeacherContactIsNull(teacherContact);
            ValidateTeacherContactRequiredFields(teacherContact);
        }

        private static void ValidateTeacherContactIsNull(TeacherContact teacherContact)
        {
            if (teacherContact is null)
            {
                throw new NullTeacherContactException();
            }
        }

        private static void ValidateTeacherContactRequiredFields(TeacherContact teacherContact)
        {
            switch (teacherContact)
            {
                case { } when IsInvalid(teacherContact.TeacherId):
                    throw new InvalidTeacherContactException(
                        parameterName: nameof(TeacherContact.TeacherId),
                        parameterValue: teacherContact.TeacherId);

                case { } when IsInvalid(teacherContact.ContactId):
                    throw new InvalidTeacherContactException(
                        parameterName: nameof(TeacherContact.ContactId),
                        parameterValue: teacherContact.ContactId);
            }
        }

        private static void ValidateTeacherContactIdIsNull(Guid teacherId, Guid contactId)
        {
            if (teacherId == default)
            {
                throw new InvalidTeacherContactException(
                    parameterName: nameof(TeacherContact.TeacherId),
                    parameterValue: teacherId);
            }

            if (contactId == default)
            {
                throw new InvalidTeacherContactException(
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

        private static bool IsInvalid(Guid input) => input == default;
    }
}