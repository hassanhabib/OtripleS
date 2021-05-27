using System;
using System.Linq;
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
            catch (Exception exception)
            {
                throw CreateAndLogServiceException(exception);
            }
        }

        private StudentRegistrationDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentRegistrationDependencyException = new StudentRegistrationDependencyException(exception);
            this.loggingBroker.LogCritical(StudentRegistrationDependencyException);

            return StudentRegistrationDependencyException;
        }

        private StudentRegistrationServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentRegistrationServiceException = new StudentRegistrationServiceException(exception);
            this.loggingBroker.LogError(studentRegistrationServiceException);

            return studentRegistrationServiceException;
        }
    }
}
