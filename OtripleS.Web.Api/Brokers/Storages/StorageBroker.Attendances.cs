// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Attendances;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Attendance> Attendances { get; set; }

        public async ValueTask<Attendance> InsertAttendanceAsync(Attendance attendance)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Attendance> attendanceEntityEntry = await broker.Attendances.AddAsync(entity: attendance);
            await broker.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }

        public IQueryable<Attendance> SelectAllAttendances() => this.Attendances.AsQueryable();

        public async ValueTask<Attendance> SelectAttendanceByIdAsync(Guid attendanceId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Attendances.FindAsync(attendanceId);
        }

        public async ValueTask<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Attendance> attendanceEntityEntry = broker.Attendances.Update(entity: attendance);
            await broker.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }

        public async ValueTask<Attendance> DeleteAttendanceAsync(Attendance attendance)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Attendance> attendanceEntityEntry = broker.Attendances.Remove(entity: attendance);
            await broker.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }
    }
}
