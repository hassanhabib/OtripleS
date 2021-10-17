// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.Exams
{
    public partial class ExamService
    {
        private static void ValidateExamId(Guid id) =>
            Validate((Rule: IsInvalid(id), Parameter: nameof(Exam.Id)));

        private void ValidateExamOnAdd(Exam exam)
        {
            ValidateExamIsNotNull(exam);

            Validate(
                (Rule: IsInvalid(exam.Id), Parameter: nameof(Exam.Id)),
                (Rule: IsInvalid(exam.Type), Parameter: nameof(Exam.Type)),
                (Rule: IsInvalid(exam.CreatedBy), Parameter: nameof(Exam.CreatedBy)),
                (Rule: IsInvalid(exam.CreatedDate), Parameter: nameof(Exam.CreatedDate)),
                (Rule: IsInvalid(exam.UpdatedBy), Parameter: nameof(Exam.UpdatedBy)),
                (Rule: IsInvalid(exam.UpdatedDate), Parameter: nameof(Exam.UpdatedDate)),

                (Rule: IsNotSame(
                    firstId: exam.UpdatedBy,
                    secondId: exam.CreatedBy,
                    secondIdName: nameof(Exam.CreatedBy)),
                Parameter: nameof(Exam.UpdatedBy)),

                (Rule: IsNotSame(
                    firstDate: exam.UpdatedDate,
                    secondDate: exam.CreatedDate,
                    secondDateName: nameof(Exam.CreatedDate)),
                Parameter: nameof(Exam.UpdatedDate)),

                (Rule: IsNotRecent(exam.CreatedDate), Parameter: nameof(Exam.CreatedDate)));
        }

        private void ValidateExamOnModify(Exam exam)
        {
            ValidateExamIsNotNull(exam);

            Validate(
                (Rule: IsInvalid(exam.Id), Parameter: nameof(Exam.Id)),
                (Rule: IsInvalid(exam.Type), Parameter: nameof(Exam.Type)),
                (Rule: IsInvalid(exam.CreatedBy), Parameter: nameof(Exam.CreatedBy)),
                (Rule: IsInvalid(exam.CreatedDate), Parameter: nameof(Exam.CreatedDate)),
                (Rule: IsInvalid(exam.UpdatedBy), Parameter: nameof(Exam.UpdatedBy)),
                (Rule: IsInvalid(exam.UpdatedDate), Parameter: nameof(Exam.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: exam.UpdatedDate,
                    secondDate: exam.CreatedDate,
                    secondDateName: nameof(Exam.CreatedDate)),
                Parameter: nameof(Exam.UpdatedDate)),

                (Rule: IsNotRecent(exam.UpdatedDate), Parameter: nameof(Exam.UpdatedDate)));
        }

        private static void ValidateExamIsNotNull(Exam exam)
        {
            if (exam is null)
            {
                throw new NullExamException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid(ExamType type) => new
        {
            Condition = Enum.IsDefined(type) == false,
            Message = "Value is not recognized"
        };

        private static dynamic IsNotSame(
        Guid firstId,
        Guid secondId,
        string secondIdName) => new
        {
            Condition = firstId != secondId,
            Message = $"Id is not same as {secondIdName}"
        };

        private static dynamic IsNotSame(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) => new
        {
            Condition = firstDate != secondDate,
            Message = $"Date is not same as {secondDateName}"
        };

        private static dynamic IsSame(
        DateTimeOffset firstDate,
        DateTimeOffset secondDate,
        string secondDateName) => new
        {
            Condition = firstDate == secondDate,
            Message = $"Date is same as {secondDateName}"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private bool IsDateNotRecent(DateTimeOffset dateTime)
        {
            DateTimeOffset now = this.dateTimeBroker.GetCurrentDateTime();
            int oneMinute = 1;
            TimeSpan difference = now.Subtract(dateTime);

            return Math.Abs(difference.TotalMinutes) > oneMinute;
        }

        private void ValidateStorageExams(IQueryable<Exam> storageExams)
        {
            if (!storageExams.Any())
            {
                this.loggingBroker.LogWarning("No exams found in storage.");
            }
        }

        private static void ValidateStorageExam(Exam storageExam, Guid examId)
        {
            if (storageExam == null)
            {
                throw new NotFoundExamException(examId);
            }
        }

        private static void ValidateAgainstStorageExamOnModify(Exam inputExam, Exam storageExam)
        {
            switch (inputExam)
            {
                case { } when inputExam.CreatedDate != storageExam.CreatedDate:
                    throw new InvalidExamException(
                        parameterName: nameof(Exam.CreatedDate),
                        parameterValue: inputExam.CreatedDate);

                case { } when inputExam.CreatedBy != storageExam.CreatedBy:
                    throw new InvalidExamException(
                        parameterName: nameof(Exam.CreatedBy),
                        parameterValue: inputExam.CreatedBy);

                case { } when inputExam.UpdatedDate == storageExam.UpdatedDate:
                    throw new InvalidExamException(
                        parameterName: nameof(Exam.UpdatedDate),
                        parameterValue: inputExam.UpdatedDate);
            }
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidExamException = new InvalidExamException();

            foreach((dynamic rule, string paramter) in validations)
            {
                if(rule.Condition)
                {
                    invalidExamException.UpsertDataList(
                        key: paramter,
                        value: rule.Message);
                }
            }

            invalidExamException.ThrowIfContainsErrors();
        }
    }
}
