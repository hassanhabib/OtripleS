// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Teacher> Teachers { get; set; }

        public async ValueTask<Teacher> InsertTeacherAsync(Teacher teacher)
        {
            EntityEntry<Teacher> teacherEntityEntry = await this.Teachers.AddAsync(teacher);
            await this.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }

        public IQueryable<Teacher> SelectAllTeachers() => this.Teachers.AsQueryable();

        public async ValueTask<Teacher> SelectTeacherByIdAsync(Guid teacherId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Teachers.FindAsync(teacherId);
        }

        public async ValueTask<Teacher> UpdateTeacherAsync(Teacher teacher)
        {
            EntityEntry<Teacher> teacherEntityEntry = this.Teachers.Update(teacher);
            await this.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }

        public async ValueTask<Teacher> DeleteTeacherAsync(Teacher teacher)
        {
            EntityEntry<Teacher> teacherEntityEntry = this.Teachers.Remove(teacher);
            await this.SaveChangesAsync();

            return teacherEntityEntry.Entity;
        }
    }
}
