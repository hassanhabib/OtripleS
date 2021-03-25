using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Fees;

namespace OtripleS.Web.Api.Services.Fees
{
    public partial class FeeService
    {
        private void ValidateStorageFees(IQueryable<Fee> storageFees)
        {
            if (storageFees.Count() == 0)
            {
                this.loggingBroker.LogWarning("No fees found in storage.");
            }
        }
    }
}
