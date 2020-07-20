using Microsoft.EntityFrameworkCore;

using OtripleS.Web.Api.Models.Classrooms;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Classroom> Classrooms { get; set; }
    }
}
