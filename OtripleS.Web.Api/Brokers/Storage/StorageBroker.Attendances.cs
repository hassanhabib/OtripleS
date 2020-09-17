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

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Attendance> Attendances { get; set; }
        public async ValueTask<Attendance> InsertAttendanceAsync(Attendance attendance)
        {
            EntityEntry<Attendance> attendanceEntityEntry = await this.Attendances.AddAsync(attendance);
            await this.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }

        public IQueryable<Attendance> SelectAllAttendances() => this.Attendances.AsQueryable();

        public async ValueTask<Attendance> SelectAttendanceByIdAsync(Guid attendanceId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Attendances.FindAsync(attendanceId);
        }

        public async ValueTask<Attendance> UpdateAttendanceAsync(Attendance attendance)
        {
            EntityEntry<Attendance> attendanceEntityEntry = this.Attendances.Update(attendance);
            await this.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }

        public async ValueTask<Attendance> DeleteAttendanceAsync(Attendance attendance)
        {
            EntityEntry<Attendance> attendanceEntityEntry = this.Attendances.Remove(attendance);
            await this.SaveChangesAsync();

            return attendanceEntityEntry.Entity;
        }
    }
}
