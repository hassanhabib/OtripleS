// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attendances;

namespace OtripleS.Web.Api.Services.Attendances
{
    public interface IAttendanceService
    {
        ValueTask<Attendance> ModifyAttendanceAsync(Attendance attendance);
        ValueTask<Attendance> CreateAttendanceAsync(Attendance attendance);
        ValueTask<Attendance> RetrieveAttendanceByIdAsync(Guid attendanceId);
        IQueryable<Attendance> RetrieveAllAttendances();
        ValueTask<Attendance> DeleteAttendanceAsync(Guid attendanceId);
    }
}
