using EFxceptions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using OtripleS.Web.Api.Models.Students;

using System.Linq;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private readonly IConfiguration configuration;

        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        public IQueryable<Student> SelectAllStudents() => this.Students.AsQueryable();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
