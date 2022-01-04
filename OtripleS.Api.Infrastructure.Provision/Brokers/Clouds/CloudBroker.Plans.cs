// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using OS = Microsoft.Azure.Management.AppService.Fluent.OperatingSystem;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<IAppServicePlan> CreatePlanAsync(
            string planName,
            IResourceGroup resourceGroup)
        {
            return await this.azure.AppServices.AppServicePlans
                .Define(planName)
                .WithRegion(Region.USWest2)
                .WithExistingResourceGroup(resourceGroup)
                .WithPricingTier(PricingTier.StandardS1)
                .WithOperatingSystem(OS.Windows)
                .CreateAsync();
        }
    }
}
