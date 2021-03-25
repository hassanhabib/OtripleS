// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
<<<<<<< HEAD
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
=======
using System.Linq;
>>>>>>> origin/master
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
<<<<<<< HEAD
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
=======
        private delegate IQueryable<Fee> ReturningQueryableFeeFunction();

        private IQueryable<Fee> TryCatch(ReturningQueryableFeeFunction returningQueryableFeeFunction)
        {
            try
            {
                return returningQueryableFeeFunction();
>>>>>>> origin/master
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
<<<<<<< HEAD
            catch (DuplicateKeyException duplicateKeyException)
            {
                var alreadyExistsFeeException =
                    new AlreadyExistsFeeException(duplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsFeeException);
            }
=======
>>>>>>> origin/master
            catch (DbUpdateException dbUpdateException)
            {
                throw CreateAndLogDependencyException(dbUpdateException);
            }
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

<<<<<<< HEAD
        private FeeValidationException CreateAndLogValidationException(Exception exception)
        {
            var feeValidationException = new FeeValidationException(exception);
            this.loggingBroker.LogError(feeValidationException);

            return feeValidationException;
        }

=======
>>>>>>> origin/master
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

        private FeeServiceException CreateAndLogServiceException(Exception exception)
        {
            var feeServiceException = new FeeServiceException(exception);
            this.loggingBroker.LogError(feeServiceException);

            return feeServiceException;
        }
<<<<<<< HEAD
=======

>>>>>>> origin/master
    }
}
