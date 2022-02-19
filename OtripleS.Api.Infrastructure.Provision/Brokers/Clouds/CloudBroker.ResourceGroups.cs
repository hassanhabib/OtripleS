// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName) =>
           await this.azure.ResourceGroups.ContainAsync(resourceGroupName);

        public async ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName)
        {
            return await this.azure.ResourceGroups
                .Define(name: resourceGroupName)
                .WithRegion(region: Region.USWest2)
                .CreateAsync();
        }

        public async ValueTask DeleteResourceGroupAsync(string resourceGroupName) =>
            await this.azure.ResourceGroups.DeleteByNameAsync(resourceGroupName);
    }
}
