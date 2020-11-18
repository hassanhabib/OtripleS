// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;

namespace OtripleS.Web.Api.Services.StudentExams
{
    public partial class StudentExamService
    {
        private delegate ValueTask<StudentExam> ReturningStudentExamFunction();
        private delegate IQueryable<StudentExam> ReturningQueryableStudentExamFunction();

        private async ValueTask<StudentExam> TryCatch(ReturningStudentExamFunction returningStudentExamFunction)
        {
            try
            {
                return await returningStudentExamFunction();
            }
            catch (NullStudentExamException nullStudentExamException)
            {
                throw CreateAndLogValidationException(nullStudentExamException);
            }
            catch (InvalidStudentExamException invalidStudentExamInputException)
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
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentExamException =
                    new AlreadyExistsStudentExamException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentExamException);
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
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentExamServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentServiceException = new StudentExamServiceException(exception);
            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }

        private StudentExamDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentDependencyException = new StudentExamDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private StudentExamDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentDependencyException = new StudentExamDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private StudentExamValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentValidationException = new StudentExamValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }
    }
}
