// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attendances;

namespace OtripleS.Web.Api.Services.Foundations.Attendances
{
    public interface IAttendanceService
    {
        ValueTask<Attendance> CreateAttendanceAsync(Attendance attendance);
        IQueryable<Attendance> RetrieveAllAttendances();
        ValueTask<Attendance> RetrieveAttendanceByIdAsync(Guid attendanceId);
        ValueTask<Attendance> ModifyAttendanceAsync(Attendance attendance);
        ValueTask<Attendance> RemoveAttendanceByIdAsync(Guid attendanceId);
    }
}
