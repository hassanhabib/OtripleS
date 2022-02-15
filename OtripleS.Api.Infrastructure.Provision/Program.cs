// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using OtripleS.Web.Api.Infrastructure.Provision.Services.Proccesings.CloudManagements;

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