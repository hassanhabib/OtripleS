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

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Classroom> Classrooms { get; set; }

        public async ValueTask<Classroom> InsertClassroomAsync(Classroom Classroom) =>
            await InsertClassroomAsync(Classroom);

        public IQueryable<Classroom> SelectAllClassrooms() => this.Classrooms;

        public async ValueTask<Classroom> SelectClassroomByIdAsync(Guid classroomId)
        {
            var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Classrooms.FindAsync(classroomId);
        }

        public async ValueTask<Classroom> UpdateClassroomAsync(Classroom classroom)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Classroom> classroomEntityEntry = broker.Classrooms.Update(entity: classroom);
            await broker.SaveChangesAsync();

            return classroomEntityEntry.Entity;
        }

        public async ValueTask<Classroom> DeleteClassroomAsync(Classroom classroom)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Classroom> classroomEntityEntry = broker.Classrooms.Remove(entity: classroom);
            await broker.SaveChangesAsync();

            return classroomEntityEntry.Entity;
        }
    }
}
