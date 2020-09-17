// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Course> Courses { get; set; }

        public async ValueTask<Course> InsertCourseAsync(Course course)
        {
            EntityEntry<Course> courseEntityEntry = await this.Courses.AddAsync(course);
            await this.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public IQueryable<Course> SelectAllCourses() => this.Courses.AsQueryable();

        public async ValueTask<Course> SelectCourseByIdAsync(Guid courseId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Courses.FindAsync(courseId);
        }

        public async ValueTask<Course> UpdateCourseAsync(Course course)
        {
            EntityEntry<Course> courseEntityEntry = this.Courses.Update(course);
            await this.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public async ValueTask<Course> DeleteCourseAsync(Course course)
        {
            EntityEntry<Course> courseEntityEntry = this.Courses.Remove(course);
            await this.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }
    }
}
