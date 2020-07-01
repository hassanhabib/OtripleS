using System;
using System.Threading.Tasks;
using System.Linq;
using OtripleS.Web.Api.Models.Students;
using Microsoft.EntityFrameworkCore;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public async ValueTask<Student> SelectStudentByIdAsync(Guid studentId) => Students.FindAsync(studentId);
        public IQueryable<Student> SelectAllStudents() => this.Students.AsQueryable();
    }
}
