// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private delegate ValueTask<Fee> ReturningFeeFunction();

        private async ValueTask<Fee> TryCatch(ReturningFeeFunction returningFeeFunction)
        {
            try
            {
                return await returningFeeFunction();
            }
            catch (NullFeeException nullFeeException)
            {
                throw CreateAndLogValidationException(nullFeeException);
            }
            catch (InvalidFeeException invalidFeeInputException)
            {
                throw CreateAndLogValidationException(invalidFeeInputException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsFeeException =
                    new AlreadyExistsFeeException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsFeeException);
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
        }

        private FeeValidationException CreateAndLogValidationException(Exception exception)
        {
            var feeValidationException = new FeeValidationException(exception);
            this.loggingBroker.LogError(feeValidationException);

            return feeValidationException;
        }

        private FeeDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var feeDependencyException = new FeeDependencyException(exception);
            this.loggingBroker.LogCritical(feeDependencyException);

            return feeDependencyException;
        }

        private FeeDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var feeDependencyException = new FeeDependencyException(exception);
            this.loggingBroker.LogError(feeDependencyException);

            return feeDependencyException;
        }
    }
}
