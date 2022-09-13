// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public async ValueTask<Student> InsertStudentAsync(Student student)
        {
            var broker = new StorageBroker(this.configuration);
            EntityEntry<Student> studentEntityEntry = await broker.Students.AddAsync(entity: student);
            await broker.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }

        public IQueryable<Student> SelectAllStudents() => this.Students;

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId)
        {
            using var broker = new StorageBroker(this.configuration);
            broker.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await broker.Students.FindAsync(studentId);
        }

        public async ValueTask<Student> UpdateStudentAsync(Student student)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Student> studentEntityEntry = broker.Students.Update(entity: student);
            await broker.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }

        public async ValueTask<Student> DeleteStudentAsync(Student student)
        {
            using var broker = new StorageBroker(this.configuration);
            EntityEntry<Student> studentEntityEntry = broker.Students.Remove(entity: student);
            await broker.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }
    }
}
