// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;

namespace OtripleS.Web.Api.Services.StudentExams
{
    public partial class StudentExamService
    {
        private delegate ValueTask<StudentExam> ReturningStudentExamFunction();

        private async ValueTask<StudentExam> TryCatch(
            ReturningStudentExamFunction returningStudentExamFunction)
        {
            try
            {
                return await returningStudentExamFunction();
            }
            catch (InvalidStudentExamInputException invalidStudentExamInputException)
            {
                throw CreateAndLogValidationException(invalidStudentExamInputException);
            }
            catch (NotFoundStudentExamException nullStudentExamException)
            {
                throw CreateAndLogValidationException(nullStudentExamException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
        }

        private StudentExamValidationException CreateAndLogValidationException(Exception exception)
        {
            var StudentExamValidationException = new StudentExamValidationException(exception);
            this.loggingBroker.LogError(StudentExamValidationException);

            return StudentExamValidationException;
        }

        private StudentExamDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentExamDependencyException = new StudentExamDependencyException(exception);
            this.loggingBroker.LogCritical(StudentExamDependencyException);

            return StudentExamDependencyException;
        }
    }
}
