// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Guardian;
using OtripleS.Web.Api.Models.Guardian.Exceptions;

namespace OtripleS.Web.Api.Services.Guardians
{
    public partial class GuardianService
    {
        private delegate ValueTask<Guardian> ReturningGuardianFunction();

        private async ValueTask<Guardian> TryCatch(ReturningGuardianFunction returningGuardianFunction)
        {
            try
            {
                return await returningGuardianFunction();
            }
            catch (NotFoundGuardianException notFoundGuardianException)
            {
                throw CreateAndLogValidationException(notFoundGuardianException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
        }

        private GuardianValidationException CreateAndLogValidationException(Exception exception)
        {
            var GuardianValidationException = new GuardianValidationException(exception);
            this.loggingBroker.LogError(GuardianValidationException);

            return GuardianValidationException;
        }

        private void ValidateGuardianId(Guid guardianId)
        {
            if (guardianId == default)
            {
                throw new InvalidGuardianException(
                    parameterName: nameof(Guardian.Id),
                    parameterValue: guardianId);
            }
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
    }
}
