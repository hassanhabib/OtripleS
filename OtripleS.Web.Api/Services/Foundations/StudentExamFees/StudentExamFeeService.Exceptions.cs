// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeService
    {
        private delegate ValueTask<StudentExamFee> ReturningStudentExamFeeFunction();
        private delegate IQueryable<StudentExamFee> ReturningStudentExamFeesFunction();

        private async ValueTask<StudentExamFee> TryCatch(
            ReturningStudentExamFeeFunction returningStudentExamFeeFunction)
        {
            try
            {
                return await returningStudentExamFeeFunction();
            }
            catch (NullStudentExamFeeException nullStudentExamFeeException)
            {
                throw CreateAndLogValidationException(nullStudentExamFeeException);
            }
            catch (NotFoundStudentExamFeeException notFoundStudentExamFeeException)
            {
                throw CreateAndLogValidationException(notFoundStudentExamFeeException);
            }
            catch (InvalidStudentExamFeeException invalidStudentExamFeeInputException)
            {
                throw CreateAndLogValidationException(invalidStudentExamFeeInputException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsStudentExamFeeException =
                    new AlreadyExistsStudentExamFeeException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsStudentExamFeeException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedStudentExamFeeException =
                    new LockedStudentExamFeeException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedStudentExamFeeException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                var failedStudentExamFeeServiceException =
                    new FailedStudentExamFeeServiceException(exception);

                throw CreateAndLogServiceException(failedStudentExamFeeServiceException);
            }
        }

        private IQueryable<StudentExamFee> TryCatch(
            ReturningStudentExamFeesFunction returningStudentExamFeesFunction)
        {
            try
            {
                return returningStudentExamFeesFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentExamFeeValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentExamFeeValidationException = new StudentExamFeeValidationException(exception);
            this.loggingBroker.LogError(studentExamFeeValidationException);

            return studentExamFeeValidationException;
        }

        private StudentExamFeeDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentExamFeeDependencyException = new StudentExamFeeDependencyException(exception);
            this.loggingBroker.LogCritical(studentExamFeeDependencyException);

            return studentExamFeeDependencyException;
        }

        private StudentExamFeeDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentExamFeeDependencyException = new StudentExamFeeDependencyException(exception);
            this.loggingBroker.LogError(studentExamFeeDependencyException);

            return studentExamFeeDependencyException;
        }

        private StudentExamFeeServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentExamFeeServiceException = new StudentExamFeeServiceException(exception);
            this.loggingBroker.LogError(studentExamFeeServiceException);

            return studentExamFeeServiceException;
        }
    }
}
