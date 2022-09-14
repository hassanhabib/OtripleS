// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.SemesterCourses;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<SemesterCourse> SemesterCourses { get; set; }

        public async ValueTask<SemesterCourse> InsertSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<SemesterCourse> semesterCourseEntityEntry =
                await broker.SemesterCourses.AddAsync(entity: semesterCourse);

            await broker.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }

        public IQueryable<SemesterCourse> SelectAllSemesterCourses() => this.SemesterCourses;

        public async ValueTask<SemesterCourse> SelectSemesterCourseByIdAsync(Guid semesterCourseId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.SemesterCourses.FindAsync(semesterCourseId);
        }
         
        public async ValueTask<SemesterCourse> UpdateSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            var broker = new StorageBroker(this.configuration);

            EntityEntry<SemesterCourse> semesterCourseEntityEntry =
                broker.SemesterCourses.Update(entity: semesterCourse);

            await broker.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }

        public async ValueTask<SemesterCourse> DeleteSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<SemesterCourse> semesterCourseEntityEntry = broker.SemesterCourses.Remove(entity: semesterCourse);
            await broker.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }
    }
}
