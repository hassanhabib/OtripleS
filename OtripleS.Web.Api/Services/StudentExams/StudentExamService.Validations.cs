// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
    }
}
