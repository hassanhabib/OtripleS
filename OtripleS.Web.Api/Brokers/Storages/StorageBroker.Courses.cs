// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Courses;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Course> Courses { get; set; }

        public async ValueTask<Course> InsertCourseAsync(Course course)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Course> courseEntityEntry = await broker.Courses.AddAsync(entity: course);
            await broker.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public IQueryable<Course> SelectAllCourses() => this.Courses;

        public async ValueTask<Course> SelectCourseByIdAsync(Guid CourseId) =>
           await SelectCourseByIdAsync(CourseId);

        public async ValueTask<Course> UpdateCourseAsync(Course course)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Course> courseEntityEntry = broker.Courses.Update(entity: course);
            await broker.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }

        public async ValueTask<Course> DeleteCourseAsync(Course course)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Course> courseEntityEntry = broker.Courses.Remove(entity: course);
            await broker.SaveChangesAsync();

            return courseEntityEntry.Entity;
        }
    }
}
