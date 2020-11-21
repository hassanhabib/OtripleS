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
                throw new InvalidStudentExamInputException(
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

        private void ValidateStorageStudentExams(IQueryable<StudentExam> studentExams)
        {
            if (studentExams.Count() == 0)
            {
                this.loggingBroker.LogWarning("No Student Exams found in storage.");
            }
        }

        private void ValidateStudentExamOnModify(StudentExam studentExam)
        {
            ValidateStudentExamIsNotNull(studentExam);
            ValidateStudentExamId(studentExam.Id);
            ValidateStudentExam(studentExam);
            ValidateInvalidAuditFields(studentExam);
        }

        private void ValidateStudentExam(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.StudentId):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.StudentId),
                        parameterValue: studentExam.StudentId);

                case { } when IsInvalid(studentExam.ExamId):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.ExamId),
                        parameterValue: studentExam.ExamId);
            }
        }

        private void ValidateInvalidAuditFields(StudentExam studentExam)
        {
            switch (studentExam)
            {
                case { } when IsInvalid(studentExam.CreatedBy):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.CreatedBy),
                        parameterValue: studentExam.CreatedBy);

                case { } when IsInvalid(studentExam.UpdatedBy):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.UpdatedBy),
                        parameterValue: studentExam.UpdatedBy);

                case { } when IsInvalid(studentExam.CreatedDate):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.CreatedDate),
                        parameterValue: studentExam.CreatedDate);

                case { } when IsInvalid(studentExam.UpdatedDate):
                    throw new InvalidStudentExamInputException(
                        parameterName: nameof(StudentExam.UpdatedDate),
                        parameterValue: studentExam.UpdatedDate);
            }
        }

        private bool IsInvalid(DateTimeOffset inputDate) => inputDate == default;

        private bool IsInvalid(Guid input) => input == default;
 
        private void ValidateStudentExamIsNotNull(StudentExam studentExam)
        {
            if (studentExam is null)
            {
                throw new NullStudentExamException();
            }
        }
    }
}
