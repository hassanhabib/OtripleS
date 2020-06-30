using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public ValueTask<Student> SelectStudentById(Guid studentId) => Students.FindAsync(studentId);
    }
}
