// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.StudentSemesterCourses;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentSemesterCourse> StudentSemesterCourses { get; set; }

        public async ValueTask<StudentSemesterCourse> InsertStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                await broker.StudentSemesterCourses.AddAsync(entity: studentSemesterCourse);

            await broker.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }

        public IQueryable<StudentSemesterCourse> SelectAllStudentSemesterCourses() =>
            this.StudentSemesterCourses;

        public async ValueTask<StudentSemesterCourse> SelectStudentSemesterCourseByIdAsync(
            Guid studentId, Guid semesterCourseId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.StudentSemesterCourses.FindAsync(studentId, semesterCourseId);
        }

        public async ValueTask<StudentSemesterCourse> UpdateStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                broker.StudentSemesterCourses.Update(entity: studentSemesterCourse);

            await broker.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }

        public async ValueTask<StudentSemesterCourse> DeleteStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            using var broker = new StorageBroker(this.configuration);

            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                broker.StudentSemesterCourses.Remove(entity: studentSemesterCourse);

            await broker.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }
    }
}
