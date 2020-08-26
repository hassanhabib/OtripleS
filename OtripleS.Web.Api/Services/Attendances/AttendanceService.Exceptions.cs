// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            catch (NotFoundAttendanceException notFoundAttendanceException)
            {
                throw CreateAndLogValidationException(notFoundAttendanceException);
            }
            catch (SqlException sqlException)
            {
                throw CreateAndLogCriticalDependencyException(sqlException);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                var lockedAttendanceException = new LockedAttendanceException(dbUpdateConcurrencyException);

                throw CreateAndLogDependencyException(lockedAttendanceException);
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

        private AttendanceDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var attendanceDependencyException = new AttendanceDependencyException(exception);
            this.loggingBroker.LogError(attendanceDependencyException);

            return attendanceDependencyException;
        }

        private AttendanceServiceException CreateAndLogServiceException(Exception exception)
        {
            var attendanceServiceException = new AttendanceServiceException(exception);
            this.loggingBroker.LogError(attendanceServiceException);

            return attendanceServiceException;
        }
    }
}
