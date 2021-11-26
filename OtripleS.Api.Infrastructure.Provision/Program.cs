// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using OtripleS.Web.Api.Infrastructure.Provision.Services.Proccesings.CloudManagements;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Infrastructure.Provision
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            ICloudManagementProcessingService cloudManagementProcessingService =
               new CloudManagementProcessingService();

            await cloudManagementProcessingService.ProcessAsync();
        }
    }
}