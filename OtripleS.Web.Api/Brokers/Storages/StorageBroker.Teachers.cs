// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Teacher> Teachers { get; set; }

        public async ValueTask<Teacher> InsertTeacherAsync(Teacher teacher)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Teacher> teacherEntityEntry = await broker.Teachers.AddAsync(entity: teacher);
            await broker.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }

        public IQueryable<Teacher> SelectAllTeachers() => this.Teachers;

        public async ValueTask<Teacher> SelectTeacherByIdAsync(Guid teacherId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Teachers.FindAsync(teacherId);
        }

        public async ValueTask<Teacher> UpdateTeacherAsync(Teacher teacher)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Teacher> teacherEntityEntry = broker.Teachers.Update(entity: teacher);
            await broker.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }

        public async ValueTask<Teacher> DeleteTeacherAsync(Teacher teacher)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Teacher> teacherEntityEntry = broker.Teachers.Remove(entity: teacher);
            await broker.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }
    }
}
