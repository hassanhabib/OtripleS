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

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<StudentSemesterCourse> StudentSemesterCourses { get; set; }

        public async ValueTask<StudentSemesterCourse> InsertStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                await this.StudentSemesterCourses.AddAsync(studentSemesterCourse);

            await this.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }

        public IQueryable<StudentSemesterCourse> SelectAllStudentSemesterCourses() =>
            this.StudentSemesterCourses.AsQueryable();

        public async ValueTask<StudentSemesterCourse> SelectStudentSemesterCourseByIdAsync(
            Guid studentId, Guid semesterCourseId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await this.StudentSemesterCourses.FindAsync(studentId, semesterCourseId);
        }

        public async ValueTask<StudentSemesterCourse> UpdateStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                this.StudentSemesterCourses.Update(studentSemesterCourse);

            await this.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }

        public async ValueTask<StudentSemesterCourse> DeleteStudentSemesterCourseAsync(
            StudentSemesterCourse studentSemesterCourse)
        {
            EntityEntry<StudentSemesterCourse> studentSemesterCourseEntityEntry =
                this.StudentSemesterCourses.Remove(studentSemesterCourse);

            await this.SaveChangesAsync();

            return studentSemesterCourseEntityEntry.Entity;
        }
    }
}
