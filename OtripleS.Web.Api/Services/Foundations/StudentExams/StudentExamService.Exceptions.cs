// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentExams
{
    public partial class StudentExamService
    {
        private delegate ValueTask<StudentExam> ReturningStudentExamFunction();
        private delegate IQueryable<StudentExam> ReturningStudentExamsFunction();

        private async ValueTask<StudentExam> TryCatch(
            ReturningStudentExamFunction returningStudentExamFunction)
        {
            try
            {
                return await returningStudentExamFunction();
            }
            catch (NullStudentExamException nullStudentExamException)
            {
                throw CreateAndLogValidationException(nullStudentExamException);
            }
            catch (InvalidStudentExamException invalidStudentExamException)
            {
                throw CreateAndLogValidationException(invalidStudentExamException);
            }
            catch (NotFoundStudentExamException nullStudentExamException)
            {
                throw CreateAndLogValidationException(nullStudentExamException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedStudentExamException = new LockedStudentExamException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedStudentExamException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedStudentExamServiceException =
                    new FailedStudentExamServiceException(exception);

                throw CreateAndLogServiceException(failedStudentExamServiceException);
            }
        }

        private IQueryable<StudentExam> TryCatch(ReturningStudentExamsFunction returningStudentExamsFunction)
        {
            try
            {
                return returningStudentExamsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedStudentExamServiceException =
                    new FailedStudentExamServiceException(exception);

                throw CreateAndLogServiceException(failedStudentExamServiceException);
            }
        }

        private StudentExamValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentExamValidationException = new StudentExamValidationException(exception);
            this.loggingBroker.LogError(studentExamValidationException);

            return studentExamValidationException;
        }

        private StudentExamDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentExamDependencyException = new StudentExamDependencyException(exception);
            this.loggingBroker.LogCritical(studentExamDependencyException);

            return studentExamDependencyException;
        }

        private StudentExamDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentExamDependencyException = new StudentExamDependencyException(exception);
            this.loggingBroker.LogError(studentExamDependencyException);

            return studentExamDependencyException;
        }

        private StudentExamServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentExamServiceException = new StudentExamServiceException(exception);
            this.loggingBroker.LogError(studentExamServiceException);

            return studentExamServiceException;
        }
    }
}
