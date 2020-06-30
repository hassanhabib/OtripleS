using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Students;
using System;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public Student GetStudent(Guid StudentId) => Students.Find(StudentId);
    }
}
