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
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xeptions;

namespace OtripleS.Web.Api.Services.Foundations.Exams
{
    public partial class ExamService
    {
        private delegate ValueTask<Exam> ReturningExamFunction();
        private delegate IQueryable<Exam> ReturningQueryableExamFunction();

        private async ValueTask<Exam> TryCatch(ReturningExamFunction returningExamFunction)
        {
            try
            {
                return await returningExamFunction();
            }
            catch (NullExamException nullExamException)
            {
                throw CreateAndLogValidationException(nullExamException);
            }
            catch (InvalidExamException invalidExamInputException)
            {
                throw CreateAndLogValidationException(invalidExamInputException);
            }
            catch (NotFoundExamException nullExamException)
            {
                throw CreateAndLogValidationException(nullExamException);
            }
            catch (SqlException sqlException)
            {
                var failedExamStorageException =
                    new FailedExamStorageException(sqlException);

                throw CreateAndLogCriticalDependencyException(failedExamStorageException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsExamException =
                    new AlreadyExistsExamException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsExamException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedExamException = new LockedExamException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedExamException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                var failedExamStorageException =
                    new FailedExamStorageException(dbUpdateException);

                throw CreateAndLogDependencyException(failedExamStorageException);
            }
            catch (Exception exception)
            {
                var failedExamServiceException =
                    new FailedExamServiceException(exception);

                throw CreateAndLogServiceException(failedExamServiceException);
            }
        }

        private IQueryable<Exam> TryCatch(ReturningQueryableExamFunction returningQueryableExamFunction)
        {
            try
            {
                return returningQueryableExamFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (Exception exception)
            {
                var failedExamServiceException =
                    new FailedExamServiceException(exception);

                throw CreateAndLogServiceException(failedExamServiceException);
            }
        }

        private ExamValidationException CreateAndLogValidationException(Exception exception)
        {
            var examValidationException = new ExamValidationException(exception);
            this.loggingBroker.LogError(examValidationException);

            return examValidationException;
        }

        private ExamDependencyException CreateAndLogCriticalDependencyException(Xeption exception)
        {
            var examDependencyException = new ExamDependencyException(exception);
            this.loggingBroker.LogCritical(examDependencyException);

            return examDependencyException;
        }   
        private ExamDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var examDependencyException = new ExamDependencyException(exception);
            this.loggingBroker.LogCritical(examDependencyException);

            return examDependencyException;
        }

        private ExamDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var examDependencyException = new ExamDependencyException(exception);
            this.loggingBroker.LogError(examDependencyException);

            return examDependencyException;
        }

        private ExamServiceException CreateAndLogServiceException(Xeption exception)
        {
            var examServiceException = new ExamServiceException(exception);
            this.loggingBroker.LogError(examServiceException);

            return examServiceException;
        }
    }
}
