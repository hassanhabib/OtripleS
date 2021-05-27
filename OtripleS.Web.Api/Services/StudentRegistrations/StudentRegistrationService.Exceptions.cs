using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;

namespace OtripleS.Web.Api.Services.StudentRegistrations
{
    public partial class StudentRegistrationService
    {
        private delegate IQueryable<StudentRegistration>
            ReturningQueryableStudentRegistrationFunction();

        private IQueryable<StudentRegistration> TryCatch
            (ReturningQueryableStudentRegistrationFunction returningQueryableStudentRegistrationFunction)
        {
            try
            {
                return returningQueryableStudentRegistrationFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
        }

        private StudentRegistrationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var feeDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogCritical(feeDependencyException);

            return feeDependencyException;
        }
    }
}
