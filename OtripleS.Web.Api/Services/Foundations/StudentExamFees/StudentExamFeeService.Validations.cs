//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeService
    {
        private static void ValidateStorageStudentExamFee(
          StudentExamFee storageStudentExamFee,
          Guid studentId,
          Guid examFeeId)
        {
            if (storageStudentExamFee == null)
                throw new NotFoundStudentExamFeeException(studentId, examFeeId);
        }

        private void ValidateStudentExamFeeOnCreate(StudentExamFee studentExamFee)
        {
            ValidateStudentExamFeeIsNull(studentExamFee);

            ValidateStudentExamFeeIdsAreNull(
                studentExamFee.StudentId,
                studentExamFee.ExamFeeId);

            ValidateInvalidAuditFields(studentExamFee);
            ValidateInvalidAuditFieldsOnCreate(studentExamFee);
        }

        private void ValidateStudentExamFeeOnModify(StudentExamFee studentExamFee)
        {
            ValidateStudentExamFeeIsNull(studentExamFee);

            ValidateStudentExamFeeIdsAreNull(
                studentExamFee.StudentId,
                studentExamFee.ExamFeeId);

            ValidateInvalidAuditFields(studentExamFee);
            ValidateInvalidAuditFieldsOnUpdate(studentExamFee);
        }

        private static void ValidateStudentExamFeeIsNull(StudentExamFee studentExamFee)
        {
            if (studentExamFee is null)
            {
                throw new NullStudentExamFeeException();
            }
        }

        private static void ValidateStudentExamFeeIdsAreNull(
            Guid studentId,
            Guid examFeeId)
        {
            if (studentId == default)
            {
                throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.StudentId),
                    parameterValue: studentId);
            }
            else if (examFeeId == default)
            {
                throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.ExamFeeId),
                    parameterValue: examFeeId);
            }
        }

        private static void ValidateInvalidAuditFields(StudentExamFee studentExamFee)
        {
            switch (studentExamFee)
            {
                case { } when IsInvalid(studentExamFee.CreatedBy):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.CreatedBy),
                    parameterValue: studentExamFee.CreatedBy);

                case { } when IsInvalid(studentExamFee.UpdatedBy):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedBy),
                    parameterValue: studentExamFee.UpdatedBy);

                case { } when IsInvalid(studentExamFee.CreatedDate):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.CreatedDate),
                    parameterValue: studentExamFee.CreatedDate);

                case { } when IsInvalid(studentExamFee.UpdatedDate):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedDate),
                    parameterValue: studentExamFee.UpdatedDate);
            }
        }

        private void ValidateInvalidAuditFieldsOnCreate(StudentExamFee studentExamFee)
        {
            switch (studentExamFee)
            {
                case { } when studentExamFee.UpdatedBy != studentExamFee.CreatedBy:
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedBy),
                    parameterValue: studentExamFee.UpdatedBy);

                case { } when studentExamFee.UpdatedDate != studentExamFee.CreatedDate:
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedDate),
                    parameterValue: studentExamFee.UpdatedDate);

                case { } when IsDateNotRecent(studentExamFee.CreatedDate):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.CreatedDate),
                    parameterValue: studentExamFee.CreatedDate);
            }
        }

        private void ValidateInvalidAuditFieldsOnUpdate(StudentExamFee studentExamFee)
        {
            switch (studentExamFee)
            {
                case { } when studentExamFee.UpdatedDate == studentExamFee.CreatedDate:
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedDate),
                    parameterValue: studentExamFee.UpdatedDate);

                case { } when IsDateNotRecent(studentExamFee.UpdatedDate):
                    throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.UpdatedDate),
                    parameterValue: studentExamFee.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageStudentExamFeeOnModify(
            StudentExamFee inputStudentExamFee,
            StudentExamFee storageStudentExamFee)
        {
            if (inputStudentExamFee.CreatedDate != storageStudentExamFee.CreatedDate)
                throw new InvalidStudentExamFeeException(
                    parameterName: nameof(StudentExamFee.CreatedDate),
                    parameterValue: inputStudentExamFee.CreatedDate);
        }

        private void ValidateStorageStudentExamFees(IQueryable<StudentExamFee> storageStudentExamFees)
        {
            if (!storageStudentExamFees.Any())
            {
                this.loggingBroker.LogWarning("No student exam fees found in storage.");
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
        private static bool IsInvalid(DateTimeOffset input) => input == default;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }
    }
}
