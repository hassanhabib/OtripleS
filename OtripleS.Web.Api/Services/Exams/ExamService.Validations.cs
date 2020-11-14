// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;

namespace OtripleS.Web.Api.Services.Exams
{
    public partial class ExamService
    {
        private void ValidateExamOnAdd(Exam exam)
        {
            ValidateExamIdIsNotNull(exam);
            ValidateExamId(exam.Id);
            ValidateExamType(exam);
            ValidateExamAuditFieldsOnCreate(exam);
        }


        private void ValidateStorageExams(IQueryable<Exam> storageExams)
        {
            if (storageExams.Count() == 0)
            {
                this.loggingBroker.LogWarning("No exams found in storage.");
            }
        }

        private void ValidateExamId(Guid examId)
        {
            if (IsInvalid(examId))
            {
                throw new InvalidExamInputException(
                    parameterName: nameof(Exam.Id),
                    parameterValue: examId);
            }
        }

        private void ValidateStorageExam(Exam storageExam, Guid examId)
        {
            if (storageExam == null)
            {
                throw new NotFoundExamException(examId);
            }
        }

        private void ValidateExamType(Exam exam)
        {
            if (IsInvalid(exam.Type))
            {
                throw new InvalidExamInputException(
                   parameterName: nameof(Exam.Type),
                   parameterValue: exam.Type);
            }
        }

        private void ValidateExamIdIsNotNull(Exam exam)
        {
            if (exam == default)
            {
                throw new NullExamException();
            }
        }

        private void ValidateExamAuditFieldsOnCreate(Exam exam)
        {
            switch (exam)
            {
                case { } when IsInvalid(input: exam.CreatedBy):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedBy),
                        parameterValue: exam.CreatedBy);

                case { } when IsInvalid(input: exam.CreatedDate):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedDate),
                        parameterValue: exam.CreatedDate);

                case { } when IsInvalid(input: exam.UpdatedBy):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedBy),
                        parameterValue: exam.UpdatedBy);

                case { } when IsInvalid(input: exam.UpdatedDate):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedDate),
                        parameterValue: exam.UpdatedDate);

                case { } when exam.UpdatedBy != exam.CreatedBy:
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedBy),
                        parameterValue: exam.UpdatedBy);

                case { } when exam.UpdatedDate != exam.CreatedDate:
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedDate),
                        parameterValue: exam.UpdatedDate);

                case { } when IsDateNotRecent(exam.CreatedDate):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedDate),
                        parameterValue: exam.CreatedDate);
            }
        }

        private bool IsInvalid(string input) => string.IsNullOrWhiteSpace(input);
        private bool IsInvalid(Guid input) => input == default;
        private bool IsInvalid(DateTimeOffset input) => input == default;
        private static bool IsInvalid(ExamType type) => Enum.IsDefined(type) == false;

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateExamOnModify(Exam exam)
        {
            ValidateExamIsNotNull(exam);
            ValidateExamId(exam.Id);
            ValidateExamtAuditFields(exam);
            ValidateDatesAreNotSame(exam);
            ValidateUpdatedDateIsRecent(exam);
        }

        private void ValidateUpdatedDateIsRecent(Exam exam)
        {
            if (IsDateNotRecent(exam.UpdatedDate))
            {
                throw new InvalidExamInputException(
                    parameterName: nameof(Exam.UpdatedDate),
                    parameterValue: exam.UpdatedDate);
            }
        }

        private void ValidateDatesAreNotSame(Exam exam)
        {
            if (exam.CreatedDate == exam.UpdatedDate)
            {
                throw new InvalidExamInputException(
                    parameterName: nameof(Exam.UpdatedDate),
                    parameterValue: exam.UpdatedDate);
            }
        }

        private void ValidateExamtAuditFields(Exam exam)
        {
            switch (exam)
            {
                case { } when IsInvalid(exam.CreatedBy):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedBy),
                        parameterValue: exam.CreatedBy);

                case { } when IsInvalid(exam.UpdatedBy):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedBy),
                        parameterValue: exam.UpdatedBy);

                case { } when IsInvalid(exam.CreatedDate):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedDate),
                        parameterValue: exam.CreatedDate);

                case { } when IsInvalid(exam.UpdatedDate):
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedDate),
                        parameterValue: exam.UpdatedDate);
            }
        }

        private void ValidateExamIsNotNull(Exam exam)
        {
            if (exam is null)
            {
                throw new NullExamException();
            }
        }

        private void ValidateAgainstStorageExamOnModify(Exam inputExam, Exam storageExam)
        {
            switch (inputExam)
            {
                case { } when inputExam.CreatedDate != storageExam.CreatedDate:
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedDate),
                        parameterValue: inputExam.CreatedDate);

                case { } when inputExam.CreatedBy != storageExam.CreatedBy:
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.CreatedBy),
                        parameterValue: inputExam.CreatedBy);

                case { } when inputExam.UpdatedDate == storageExam.UpdatedDate:
                    throw new InvalidExamInputException(
                        parameterName: nameof(Exam.UpdatedDate),
                        parameterValue: inputExam.UpdatedDate);
            }
        }
    }
}
