using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.Guardian;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial class StorageBroker
    {
        public DbSet<Guardian> Guardians { get; set; }
    }
}
