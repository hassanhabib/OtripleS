// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.SemesterCourses;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<SemesterCourse> SemesterCourses { get; set; }

        public async ValueTask<SemesterCourse> InsertSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            EntityEntry<SemesterCourse> semesterCourseEntityEntry =
                await this.SemesterCourses.AddAsync(semesterCourse);

            await this.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }

        public IQueryable<SemesterCourse> SelectAllSemesterCourses() =>
            this.SemesterCourses.AsQueryable();

        public async ValueTask<SemesterCourse> SelectSemesterCourseByIdAsync(Guid semesterCourseId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await SemesterCourses.FindAsync(semesterCourseId);
        }

        public async ValueTask<SemesterCourse> UpdateSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            EntityEntry<SemesterCourse> semesterCourseEntityEntry =
                this.SemesterCourses.Update(semesterCourse);

            await this.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }

        public async ValueTask<SemesterCourse> DeleteSemesterCourseAsync(SemesterCourse semesterCourse)
        {
            EntityEntry<SemesterCourse> semesterCourseEntityEntry = this.SemesterCourses.Remove(semesterCourse);
            await this.SaveChangesAsync();

            return semesterCourseEntityEntry.Entity;
        }
    }
}
