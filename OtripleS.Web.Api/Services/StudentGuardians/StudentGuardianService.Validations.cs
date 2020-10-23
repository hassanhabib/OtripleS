// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;

namespace OtripleS.Web.Api.Services.StudentGuardians
{
    public partial class StudentGuardianService
    {
        private void ValidateStudentGuardianOnModify(StudentGuardian studentGuardian)
        {
            ValidateStudentGuardianIsNull(studentGuardian);
            ValidateStudentGuardianRequiredFields(studentGuardian);
            ValidateInvalidAuditFields(studentGuardian);
            ValidateDatesAreNotSame(studentGuardian);
            ValidateUpdatedDateIsRecent(studentGuardian);
        }

        private void ValidateStudentGuardianOnCreate(StudentGuardian studentGuardian)
        {
            ValidateStudentGuardianIsNull(studentGuardian);
            ValidateStudentGuardianRequiredFields(studentGuardian);
            ValidateInvalidAuditFields(studentGuardian);
            ValidateInvalidAuditFieldsOnCreate(studentGuardian);
        }

        private void ValidateInvalidAuditFieldsOnCreate(StudentGuardian studentGuardian)
        {
            switch (studentGuardian)
            {
                case { } when studentGuardian.UpdatedBy != studentGuardian.CreatedBy:
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.UpdatedBy),
                        parameterValue: studentGuardian.UpdatedBy);

                case { } when studentGuardian.UpdatedDate != studentGuardian.CreatedDate:
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.UpdatedDate),
                        parameterValue: studentGuardian.UpdatedDate);

                case { } when IsDateNotRecent(studentGuardian.CreatedDate):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.CreatedDate),
                        parameterValue: studentGuardian.CreatedDate);
            }
        }

        private void ValidateStudentGuardianRequiredFields(StudentGuardian studentGuardian)
        {
            switch (studentGuardian)
            {
                case { } when IsInvalid(studentGuardian.StudentId):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.StudentId),
                        parameterValue: studentGuardian.StudentId);

                case { } when IsInvalid(studentGuardian.GuardianId):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.GuardianId),
                        parameterValue: studentGuardian.GuardianId);
            }
        }

        private void ValidateStudentGuardianIdIsNull(Guid studentId, Guid guardianId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.StudentId),
                    parameterValue: studentId);
            }
            if (guardianId == default)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.GuardianId),
                    parameterValue: guardianId);
            }
        }

        private void ValidateStorageStudentGuardians(IQueryable<StudentGuardian> studentGuardians)
        {
            if (studentGuardians.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Student Guardians found in storage.");
            }
        }

        private static void ValidateStorageStudentGuardian(
            StudentGuardian storageStudentGuardian,
            Guid semesterCourseId,
            Guid studentId)
        {
            if (storageStudentGuardian == null)
            {
                throw new NotFoundStudentGuardianException(semesterCourseId, studentId);
            }
        }

        private void ValidateAgainstStorageStudentGuardianOnModify
            (StudentGuardian inputStudentGuardian, StudentGuardian storageStudentGuardian)
        {
            switch (inputStudentGuardian)
            {
                case { } when inputStudentGuardian.CreatedDate != storageStudentGuardian.CreatedDate:
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.CreatedDate),
                        parameterValue: inputStudentGuardian.CreatedDate);
            }
        }

        private void ValidateStudentGuardianIsNull(StudentGuardian studentGuardian)
        {
            if (studentGuardian is null)
            {
                throw new NullStudentGuardianException();
            }
        }
        private void ValidateStudentGuardianId(Guid studentGuardianId)
        {
            if (studentGuardianId == Guid.Empty)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.GuardianId),
                    parameterValue: studentGuardianId);
            }
        }
        private void ValidateStudentId(Guid studentId)
        {
            if (studentId == Guid.Empty)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.StudentId),
                    parameterValue: studentId);
            }
        }

        private void ValidateInvalidAuditFields(StudentGuardian studentGuardian)
        {
            switch (studentGuardian)
            {
                case { } when IsInvalid(studentGuardian.CreatedBy):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.CreatedBy),
                        parameterValue: studentGuardian.CreatedBy);

                case { } when IsInvalid(studentGuardian.UpdatedBy):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.UpdatedBy),
                        parameterValue: studentGuardian.UpdatedBy);

                case { } when IsInvalid(studentGuardian.CreatedDate):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.CreatedDate),
                        parameterValue: studentGuardian.CreatedDate);

                case { } when IsInvalid(studentGuardian.UpdatedDate):
                    throw new InvalidStudentGuardianInputException(
                        parameterName: nameof(StudentGuardian.UpdatedDate),
                        parameterValue: studentGuardian.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(StudentGuardian studentGuardian)
        {
            if (studentGuardian.CreatedDate == studentGuardian.UpdatedDate)
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.UpdatedDate),
                    parameterValue: studentGuardian.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(StudentGuardian studentGuardian)
        {
            if (IsDateNotRecent(studentGuardian.UpdatedDate))
            {
                throw new InvalidStudentGuardianInputException(
                    parameterName: nameof(StudentGuardian.UpdatedDate),
                    parameterValue: studentGuardian.UpdatedDate);
            }
        }

        private static bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(Guid input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
