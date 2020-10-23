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
using OtripleS.Web.Api.Models.Guardians;
using OtripleS.Web.Api.Models.Guardians.Exceptions;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService
    {
        private delegate ValueTask<Guardian> ReturningGuardianFunction();
        private delegate IQueryable<Guardian> ReturningQueryableGuardianFunction();

        private async ValueTask<Guardian> TryCatch(ReturningGuardianFunction returningGuardianFunction)
        {
            try
            {
                return await returningGuardianFunction();
            }
            catch (NullGuardianException nullGuardianException)
            {
                throw CreateAndLogValidationException(nullGuardianException);
            }
            catch (InvalidGuardianException invalidGuardianException)
            {
                throw CreateAndLogValidationException(invalidGuardianException);
            }
            catch (NotFoundGuardianException notFoundGuardianException)
            {
                throw CreateAndLogValidationException(notFoundGuardianException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsGuardianException =
                    new AlreadyExistsGuardianException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsGuardianException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedGuardianException = new LockedGuardianException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedGuardianException);
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

        private IQueryable<Guardian> TryCatch(ReturningQueryableGuardianFunction returningQueryableGuardianFunction)
        {
            try
            {
                return returningQueryableGuardianFunction();
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

        private GuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var GuardianValidationException = new GuardianValidationException(exception);
            this.loggingBroker.LogError(GuardianValidationException);

            return GuardianValidationException;
        }

        private GuardianDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var guardianDependencyException = new GuardianDependencyException(exception);
            this.loggingBroker.LogCritical(guardianDependencyException);

            return guardianDependencyException;
        }

        private GuardianDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var guardianDependencyException = new GuardianDependencyException(exception);
            this.loggingBroker.LogError(guardianDependencyException);

            return guardianDependencyException;
        }

        private GuardianServiceException CreateAndLogServiceException(Exception exception)
        {
            var guardianServiceException = new GuardianServiceException(exception);
            this.loggingBroker.LogError(guardianServiceException);

            return guardianServiceException;
        }
    }
}
