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
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService
    {
        private delegate IQueryable<Assignment> ReturningQueryableAssignmentFunction();
        private delegate ValueTask<Assignment> ReturningAssignmentFunction();

        private async ValueTask<Assignment> TryCatch(ReturningAssignmentFunction returningAssignmentFunction)
        {
            try
            {
                return await returningAssignmentFunction();
            }
            catch (NullAssignmentException nullAssignmentException)
            {
                throw CreateAndLogValidationException(nullAssignmentException);
            }
            catch (InvalidAssignmentException invalidAssignmentInputException)
            {
                throw CreateAndLogValidationException(invalidAssignmentInputException);
            }
            catch (NotFoundAssignmentException notFoundAssignmentException)
            {
                throw CreateAndLogValidationException(notFoundAssignmentException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsAssignmentException =
                    new AlreadyExistsAssignmentException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsAssignmentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAssignmentException = new LockedAssignmentException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAssignmentException);
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

        private IQueryable<Assignment> TryCatch(
            ReturningQueryableAssignmentFunction returningQueryableAssignmentFunction)
        {
            try
            {
                return returningQueryableAssignmentFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
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

        private AssignmentValidationException CreateAndLogValidationException(Exception exception)
        {
            var assignmentValidationException = new AssignmentValidationException(exception);
            this.loggingBroker.LogError(assignmentValidationException);

            return assignmentValidationException;
        }

        private AssignmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var assignmentDependencyException = new AssignmentDependencyException(exception);
            this.loggingBroker.LogCritical(assignmentDependencyException);

            return assignmentDependencyException;
        }

        private AssignmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var assignmentDependencyException = new AssignmentDependencyException(exception);
            this.loggingBroker.LogError(assignmentDependencyException);

            return assignmentDependencyException;
        }

        private AssignmentServiceException CreateAndLogServiceException(Exception exception)
        {
            var assignmentServiceException = new AssignmentServiceException(exception);
            this.loggingBroker.LogError(assignmentServiceException);

            return assignmentServiceException;
        }
    }
}