using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Students;

using System.Linq;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Student> Students { get; set; }

        public IQueryable<Student> SelectAllStudents() => this.Students.AsQueryable();
    }
}
