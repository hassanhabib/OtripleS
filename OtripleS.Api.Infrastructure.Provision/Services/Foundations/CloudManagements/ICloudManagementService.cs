// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System.Threading.Tasks;
using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using OtripleS.Web.Api.Infrastructure.Provision.Models.Storages;

namespace OtripleS.Web.Api.Infrastructure.Provision.Services.Foundations.CloudManagements
{
    public interface ICloudManagementService
    {
        ValueTask<IResourceGroup> ProvisionResourceGroupAsync(
            string projectName,
            string environment);

        ValueTask<IAppServicePlan> ProvisionPlanAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup);

        ValueTask<ISqlServer> ProvisionSqlServerAsync(
            string projectName,
            string environment,
            IResourceGroup resourceGroup);

        ValueTask<SqlDatabase> ProvisionSqlDatabaseAsync(
            string projectname,
            string environment,
            ISqlServer sqlServer);

        ValueTask<IWebApp> ProvisionWebAppAsync(
            string projectName,
            string environment,
            string databaseConnectionString,
            IResourceGroup resourceGroup,
            IAppServicePlan appServicePlan);

        ValueTask DeprovisionResouceGroupAsync(
            string projectName,
            string environment);
    }
}
