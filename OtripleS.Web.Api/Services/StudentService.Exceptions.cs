using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Students;
using SchoolEM.Models.Students.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService
    {
        private delegate ValueTask<Student> ReturningStudentFunction();
        private delegate IQueryable<Student> ReturningStudentsFunction();


        private async ValueTask<Student> TryCatch(ReturningStudentFunction returningStudentFunction)
        {
            try
            {
                return await returningStudentFunction();
            }
            catch (NullStudentException nullStudentException)
            {
                throw CreateAndLogValidationException(nullStudentException);
            }
            catch (InvalidStudentException invalidStudentException)
            {
                throw CreateAndLogValidationException(invalidStudentException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (NotFoundStudentException studentNotFoundException)
            {
                throw CreateAndLogValidationException(studentNotFoundException);
            }
            catch (DuplicateKeyException duplicateKeyException)
            {
                var studentAlreadyExistsException =
                    new AlreadyExistsStudentException(duplicateKeyException);

                throw CreateAndLogValidationException(studentAlreadyExistsException);
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

        private IQueryable<Student> TryCatch(ReturningStudentsFunction returningStudentsFunction)
        {
            try
            {
                return returningStudentsFunction();
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
        }

        private StudentValidationException CreateAndLogValidationException(Exception exception)
        {
            var studentValidationException = new StudentValidationException(exception);
            this.loggingBroker.LogError(studentValidationException);

            return studentValidationException;
        }

        private StudentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var studentDependencyException = new StudentDependencyException(exception);
            this.loggingBroker.LogError(studentDependencyException);

            return studentDependencyException;
        }

        private StudentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var studentDependencyException = new StudentDependencyException(exception);
            this.loggingBroker.LogCritical(studentDependencyException);

            return studentDependencyException;
        }

        private StudentServiceException CreateAndLogServiceException(Exception exception)
        {
            var studentServiceException = new StudentServiceException(exception);

            this.loggingBroker.LogError(studentServiceException);

            return studentServiceException;
        }

    }
}
