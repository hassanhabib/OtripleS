// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Fees;
using OtripleS.Web.Api.Models.Fees.Exceptions;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private delegate IQueryable<Fee> ReturningQueryableFeeFunction();

        private IQueryable<Fee> TryCatch(ReturningQueryableFeeFunction returningQueryableFeeFunction)
        {
            try
            {
                return returningQueryableFeeFunction();
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

    }
}
