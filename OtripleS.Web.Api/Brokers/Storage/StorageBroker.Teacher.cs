using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Teacher;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Teacher> Teachers { get; set; }
    }
}
