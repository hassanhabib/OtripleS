using System;
using System.Threading.Tasks;
using System.Linq;
using OtripleS.Web.Api.Models.Students;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public IQueryable<Student> SelectAllStudents() => this.Students.AsQueryable();

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            return await Students.FindAsync(studentId);
        }

        public async ValueTask<Student> DeleteStudentAsync(Student student)
        {
            EntityEntry<Student> storageStudent = this.Students.Remove(student);
            await this.SaveChangesAsync();

            return storageStudent.Entity;
        }

        public async ValueTask<Student> InsertStudentAsync(Student student)
        {
            EntityEntry<Student> storageStudent = await this.Students.AddAsync(student);
            await this.SaveChangesAsync();

            return storageStudent.Entity;
        }
    }
}
