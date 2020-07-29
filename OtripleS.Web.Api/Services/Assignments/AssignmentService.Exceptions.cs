// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Assignments;
using OtripleS.Web.Api.Models.Assignments.Exceptions;

namespace OtripleS.Web.Api.Services.Assignments
{
    public partial class AssignmentService
    {
        private delegate IQueryable<Assignment> ReturningQueryableAssignmentFunction();

        private IQueryable<Assignment> TryCatch(
            ReturningQueryableAssignmentFunction returningQueryableAssignmentFunction)
        {
            try
            {
                return returningQueryableAssignmentFunction();
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
        private AssignmentDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var assignmentDependencyException = new AssignmentDependencyException(exception);
            this.loggingBroker.LogCritical(assignmentDependencyException);

            return assignmentDependencyException;
        }
        private AssignmentDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var assignmentDependencyException = new AssignmentDependencyException(exception);
            this.loggingBroker.LogError(assignmentDependencyException);

            return assignmentDependencyException;
        }
    }
}