// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentExams
{
    public partial class StudentExamService
    {
        private static void ValidateStudentExamId(Guid studentExamId)
        {
            if (studentExamId == Guid.Empty)
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.Id),
                    parameterValue: studentExamId);
            }
        }

        private static void ValidateStorageStudentExam(StudentExam storageStudentExam, Guid studentExamId)
        {
            if (storageStudentExam == null)
            {
                throw new NotFoundStudentExamException(studentExamId);
            }
        }

        private static void ValidateInvalidAuditFields(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.CreatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedBy),
                        parameterValue: studentExam.CreatedBy);

                case { } when IsInvalid(studentExam.UpdatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.UpdatedBy),
                        parameterValue: studentExam.UpdatedBy);

                case { } when IsInvalid(studentExam.CreatedDate):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedDate),
                        parameterValue: studentExam.CreatedDate);

                case { } when IsInvalid(studentExam.UpdatedDate):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.UpdatedDate),
                        parameterValue: studentExam.UpdatedDate);
            }
        }

        private static void ValidateDatesAreNotSame(StudentExam studentExam)
        {
            if (studentExam.CreatedDate == studentExam.UpdatedDate)
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.UpdatedDate),
                    parameterValue: studentExam.UpdatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(StudentExam studentExam)
        {
            if (IsDateNotRecent(studentExam.UpdatedDate))
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.UpdatedDate),
                    parameterValue: studentExam.UpdatedDate);
            }
        }

        private static void ValidateAgainstStorageStudentExamOnModify(
            StudentExam inputStudentExam,
            StudentExam storageStudentExam)
        {
            if (inputStudentExam.CreatedDate != storageStudentExam.CreatedDate)
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.CreatedDate),
                    parameterValue: inputStudentExam.CreatedDate);
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

        private static void ValidateStudentExamIsNotNull(StudentExam studentExam)
        {
            if (studentExam is null)
            {
                throw new NullStudentExamException();
            }
        }

        private void ValidateStudentExamOnModify(StudentExam studentExam)
        {
            ValidateStudentExamIsNotNull(studentExam);
            ValidateStudentExamId(studentExam.Id);
            ValidateStudentExamFieldsOnModify(studentExam);
            ValidateInvalidAuditFields(studentExam);
            ValidateDatesAreNotSame(studentExam);
            ValidateUpdatedDateIsRecent(studentExam);
        }

        private static void ValidateStudentExamFieldsOnModify(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.StudentId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.StudentId),
                        parameterValue: studentExam.StudentId);

                case { } when IsInvalid(studentExam.ExamId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.ExamId),
                        parameterValue: studentExam.ExamId);
            }
        }

        private void ValidateStudentExamOnCreate(StudentExam studentExam)
        {
            ValidateStudentExam(studentExam);
            ValidateStudentExamId(studentExam.Id);
            ValidateStudentExamIds(studentExam);
            ValidateStudentExamStrings(studentExam);
            ValidateStudentExamDates(studentExam);
            ValidateCreatedDateIsRecent(studentExam);
        }

        public void ValidateAgianstStorageStudentExamOnModify(StudentExam inputStudentExam, StudentExam storageStudentExam)
        {
            switch (inputStudentExam)
            {
                case { } when inputStudentExam.CreatedDate != storageStudentExam.CreatedDate:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedDate),
                        parameterValue: inputStudentExam.CreatedDate);

                case { } when inputStudentExam.CreatedBy != storageStudentExam.CreatedBy:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedBy),
                        parameterValue: inputStudentExam.CreatedBy);

                case { } when inputStudentExam.UpdatedDate == storageStudentExam.UpdatedDate:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.UpdatedDate),
                        parameterValue: inputStudentExam.UpdatedDate);
            }
        }

        private static void ValidateStudentExamStrings(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.CreatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.CreatedBy),
                        parameterValue: studentExam.CreatedBy);

                case { } when IsInvalid(studentExam.Id):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Id),
                        parameterValue: studentExam.Id);

                case { } when IsInvalid(studentExam.StudentId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Student),
                        parameterValue: studentExam.Student);

                case { } when IsInvalid(studentExam.ExamId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Exam),
                        parameterValue: studentExam.ExamId);

                case { } when IsInvalid(studentExam.TeacherId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.ReviewingTeacher),
                        parameterValue: studentExam.TeacherId);
            }
        }

        private void ValidateCreatedDateIsRecent(StudentExam studentExam)
        {
            if (IsDateNotRecent(studentExam.CreatedDate))
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(studentExam.CreatedDate),
                    parameterValue: studentExam.CreatedDate);
            }
        }


        private static void ValidateStudentExamDates(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when studentExam.CreatedDate == default:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.CreatedDate),
                        parameterValue: studentExam.CreatedDate);

                case { } when studentExam.UpdatedDate == default:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.UpdatedDate),
                        parameterValue: studentExam.UpdatedDate);
            }
        }

        private static void ValidateStudentExamIds(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.CreatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.CreatedBy),
                        parameterValue: studentExam.CreatedBy);

                case { } when IsInvalid(studentExam.UpdatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.UpdatedBy),
                        parameterValue: studentExam.UpdatedBy);

                case { } when IsInvalid(studentExam.Id):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Id),
                        parameterValue: studentExam.Id);

                case { } when IsInvalid(studentExam.StudentId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Student),
                        parameterValue: studentExam.Student);

                case { } when IsInvalid(studentExam.ExamId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.Exam),
                        parameterValue: studentExam.ExamId);

                case { } when IsInvalid(studentExam.TeacherId):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(studentExam.ReviewingTeacher),
                        parameterValue: studentExam.TeacherId);
            }
        }

        private static void ValidateStudentExam(StudentExam studentExam)
        {
            if (studentExam is null)
            {
                throw new NullStudentExamException();
            }
        }
    }
}
