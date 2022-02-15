// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial interface ICloudBroker
    {
        ValueTask<IResourceGroup> CreateResourceGroupAsync(string resourceGroupName);
        ValueTask DeleteResourceGroupAsync(string resourceGroupName);
        ValueTask<bool> CheckResourceGroupExistAsync(string resourceGroupName);
    }
}
