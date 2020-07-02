using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public async ValueTask<Student> InsertStudentAsync(Student student)
        {
            EntityEntry<Student> studentEntityEntry = await this.Students.AddAsync(student);
            await this.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }

        public IQueryable<Student> SelectAllStudents() => this.Students.AsQueryable();

        public async ValueTask<Student> AddStudentAsync(Student student)
        {
            Students.Add(student);
            await SaveChangesAsync().ConfigureAwait(false);

            return student;
        }

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Students.FindAsync(studentId);
        }

        public async ValueTask<Student> UpdateStudentAsycn(Student student)
        {
            EntityEntry<Student> studentEntityEntry = this.Students.Update(student);
            await this.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }

        public async ValueTask<Student> DeleteStudentAsync(Student student)
        {
            EntityEntry<Student> studentEntityEntry = this.Students.Remove(student);
            await this.SaveChangesAsync();

            return studentEntityEntry.Entity;
        }
    }
}
