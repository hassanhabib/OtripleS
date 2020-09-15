// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Attendances;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Attendance> InsertAttendanceAsync(Attendance Classroom);
        IQueryable<Attendance> SelectAllAttendances();
        ValueTask<Attendance> SelectAttendanceByIdAsync(Guid ClassroomId);
        ValueTask<Attendance> UpdateAttendanceAsync(Attendance Classroom);
        ValueTask<Attendance> DeleteAttendanceAsync(Attendance Classroom);
    }
}
