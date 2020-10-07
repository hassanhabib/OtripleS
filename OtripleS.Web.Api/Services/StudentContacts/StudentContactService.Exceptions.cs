//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;

namespace OtripleS.Web.Api.Services.StudentContacts
{
    public partial class StudentContactService
    {
        private delegate IQueryable<StudentContact> ReturningStudentContactsFunction();

        private IQueryable<StudentContact> TryCatch(
            ReturningStudentContactsFunction returningStudentContactsFunction)
        {
            try
            {
                return returningStudentContactsFunction();
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

        private StudentContactServiceException CreateAndLogServiceException(Exception exception)
        {
            var StudentContactServiceException = new StudentContactServiceException(exception);
            this.loggingBroker.LogError(StudentContactServiceException);

            return StudentContactServiceException;
        }

        private StudentContactDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var StudentContactDependencyException = new StudentContactDependencyException(exception);
            this.loggingBroker.LogCritical(StudentContactDependencyException);

            return StudentContactDependencyException;
        }

        private StudentContactDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var StudentContactDependencyException = new StudentContactDependencyException(exception);
            this.loggingBroker.LogError(StudentContactDependencyException);

            return StudentContactDependencyException;
        }
    }
}
