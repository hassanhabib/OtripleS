// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Classrooms;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public async ValueTask<Classroom> InsertClassroomAsync(Classroom classroom)
        {
            EntityEntry<Classroom> classroomEntityEntry = await this.Classrooms.AddAsync(classroom);
            await this.SaveChangesAsync();

            return classroomEntityEntry.Entity;
        }

        public IQueryable<Classroom> SelectAllClassrooms() => this.Classrooms.AsQueryable();

        public async ValueTask<Classroom> SelectClassroomByIdAsync(Guid classroomId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Classrooms.FindAsync(classroomId);
        }

        public async ValueTask<Classroom> UpdateClassroomAsync(Classroom classroom)
        {
            EntityEntry<Classroom> classroomEntityEntry = this.Classrooms.Update(classroom);
            await this.SaveChangesAsync();

            return classroomEntityEntry.Entity;
        }

        public async ValueTask<Classroom> DeleteClassroomAsync(Classroom classroom)
        {
            EntityEntry<Classroom> classroomEntityEntry = this.Classrooms.Remove(classroom);
            await this.SaveChangesAsync();

            return classroomEntityEntry.Entity;
        }
    }
}
