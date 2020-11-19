// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;

namespace OtripleS.Web.Api.Services.StudentExams
{
     public partial class StudentExamService
    {
        private void ValidateStudentExamId(Guid studentExamId)
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

        private void ValidateStudentExamOnCreate(StudentExam studentExam)
        {
            ValidateStudentExam(studentExam);
            ValidateStudentExamId(studentExam.Id);
            ValidateStudentExamIds(studentExam);
            ValidateStudentExamStrings(studentExam);
            ValidateStudentExamDates(studentExam);
            ValidateCreatedSignature(studentExam);
            ValidateCreatedDateIsRecent(studentExam);
        }

        private void ValidateStudentExamOnModify(StudentExam StudentExam)
        {
            ValidateStudentExam(StudentExam);
            ValidateStudentExamId(StudentExam.Id);
            ValidateStudentExamStrings(StudentExam);
            ValidateStudentExamDates(StudentExam);
            ValidateStudentExamIds(StudentExam);
            ValidateDatesAreNotSame(StudentExam);
            ValidateUpdatedDateIsRecent(StudentExam);
        }

        public void ValidateAginstStorageStudentExamOnModify(StudentExam inputStudentExam, StudentExam storageStudentExam)
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

        private void ValidateStudentExamStrings(StudentExam studentExam)
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
            }
        }

        private void ValidateDatesAreNotSame(StudentExam StudentExam)
        {
            if (StudentExam.CreatedDate == StudentExam.UpdatedDate)
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.CreatedDate),
                    parameterValue: StudentExam.CreatedDate);
            }
        }

        private void ValidateCreatedDateIsRecent(StudentExam StudentExam)
        {
            if (IsDateNotRecent(StudentExam.CreatedDate))
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.CreatedDate),
                    parameterValue: StudentExam.CreatedDate);
            }
        }

        private void ValidateUpdatedDateIsRecent(StudentExam StudentExam)
        {
            if (IsDateNotRecent(StudentExam.UpdatedDate))
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.UpdatedDate),
                    parameterValue: StudentExam.UpdatedDate);
            }
        }

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateCreatedSignature(StudentExam StudentExam)
        {
            if (StudentExam.CreatedBy != StudentExam.UpdatedBy)
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.UpdatedBy),
                    parameterValue: StudentExam.UpdatedBy);
            }
            else if (StudentExam.CreatedDate != StudentExam.UpdatedDate)
            {
                throw new InvalidStudentExamException(
                    parameterName: nameof(StudentExam.UpdatedDate),
                    parameterValue: StudentExam.UpdatedDate);
            }
        }

        private void ValidateStudentExamDates(StudentExam StudentExam)
        {
            switch (StudentExam)
            {
                case { } when StudentExam.CreatedDate == default:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedDate),
                        parameterValue: StudentExam.CreatedDate);

                case { } when StudentExam.UpdatedDate == default:
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.UpdatedDate),
                        parameterValue: StudentExam.UpdatedDate);
            }
        }

        private void ValidateStudentExamIds(StudentExam StudentExam)
        {
            switch (StudentExam)
            {
                case { } when IsInvalid(StudentExam.CreatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.CreatedBy),
                        parameterValue: StudentExam.CreatedBy);

                case { } when IsInvalid(StudentExam.UpdatedBy):
                    throw new InvalidStudentExamException(
                        parameterName: nameof(StudentExam.UpdatedBy),
                        parameterValue: StudentExam.UpdatedBy);
            }
        }

        private void ValidateStudentExam(StudentExam StudentExam)
        {
            if (StudentExam is null)
            {
                throw new NullStudentExamException();
            }
        }

        private void ValidateStorageStudentExams(IQueryable<StudentExam> storageStudents)
        {
            if (storageStudents.Count() == 0)
            {
                this.loggingBroker.LogWarning("No students found in storage.");
            }
        }

        private static bool IsInvalid(Guid input) => input == default;
    }
}
