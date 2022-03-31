// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Attendances;

namespace OtripleS.Web.Api.Services.Foundations.Attendances
{
    public partial class AttendanceService : IAttendanceService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;
        public AttendanceService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }
        public ValueTask<Attendance> ModifyAttendanceAsync(Attendance attendance) =>
        TryCatch(async () =>
        {
            ValidateAttendanceOnModify(attendance);
            Attendance maybeAttendance = await storageBroker.SelectAttendanceByIdAsync(attendance.Id);
            ValidateStorageAttendance(maybeAttendance, attendance.Id);

            ValidateAgainstStorageAttendanceOnModify(
                inputAttendance: attendance,
                storageAttendance: maybeAttendance);

            return await storageBroker.UpdateAttendanceAsync(attendance);
        });
        public ValueTask<Attendance> RetrieveAttendanceByIdAsync(Guid attendanceId) =>
        TryCatch(async () =>
        {
            ValidateAttendanceId(attendanceId);

            Attendance maybeAttendance =
                await this.storageBroker.SelectAttendanceByIdAsync(attendanceId);

            ValidateStorageAttendance(maybeAttendance, attendanceId);

            return maybeAttendance;
        });
        public IQueryable<Attendance> RetrieveAllAttendances() =>
        TryCatch(() =>
        {
            IQueryable<Attendance> storageAttendances = this.storageBroker.SelectAllAttendances();

            return storageAttendances;
        });
        public ValueTask<Attendance> RemoveAttendanceByIdAsync(Guid attendanceId) =>
        TryCatch(async () =>
        {
            ValidateAttendanceId(attendanceId);

            Attendance maybeAttendance =
                 await this.storageBroker.SelectAttendanceByIdAsync(attendanceId);

            ValidateStorageAttendance(maybeAttendance, attendanceId);

            return await storageBroker.DeleteAttendanceAsync(maybeAttendance);
        });
        public ValueTask<Attendance> CreateAttendanceAsync(Attendance attendance) =>
        TryCatch(async () =>
        {
            ValidateAttendanceOnCreate(attendance);

            return await this.storageBroker.InsertAttendanceAsync(attendance);
        });
    }
}
