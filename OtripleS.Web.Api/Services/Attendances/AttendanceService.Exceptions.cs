// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;

namespace OtripleS.Web.Api.Services.Attendances
{
    public partial class AttendanceService
    {
        private delegate ValueTask<Attendance> ReturningAttendanceFunction();

        private async ValueTask<Attendance> TryCatch(ReturningAttendanceFunction returningAttendanceFunction)
        {
            try
            {
                return await returningAttendanceFunction();
            }
            catch (NullAttendanceException nullAttendanceException)
            {
                throw CreateAndLogValidationException(nullAttendanceException);
            }
            catch (InvalidAttendanceInputException invalidAttendanceInputException)
            {
                throw CreateAndLogValidationException(invalidAttendanceInputException);
            }
            catch(SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
        }

        private AttendanceValidationException CreateAndLogValidationException(Exception exception)
        {
            var attendanceValidationException = new AttendanceValidationException(exception);
            this.loggingBroker.LogError(attendanceValidationException);

            return attendanceValidationException;
        }

        private AttendanceDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var attendanceDependencyException = new AttendanceDependencyException(exception);
            this.loggingBroker.LogCritical(attendanceDependencyException);

            return attendanceDependencyException;
        }
    }
}
