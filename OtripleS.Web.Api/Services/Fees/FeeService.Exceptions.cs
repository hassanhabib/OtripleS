using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
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
        }

        private FeeDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var feeDependencyException = new FeeDependencyException(exception);
            this.loggingBroker.LogCritical(feeDependencyException);

            return feeDependencyException;
        }

    }
}
