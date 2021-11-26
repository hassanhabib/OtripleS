// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------



using Microsoft.Azure.Management.AppService.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Sql.Fluent;
using OtripleS.Web.Api.Infrastructure.Provision.Brokers.Configurations;
using OtripleS.Web.Api.Infrastructure.Provision.Models.Configurations;
using OtripleS.Web.Api.Infrastructure.Provision.Models.Storages;
using OtripleS.Web.Api.Infrastructure.Provision.Services.Foundations.CloudManagements;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Infrastructure.Provision.Services.Proccesings.CloudManagements
{
    public class CloudManagementProcessingService : ICloudManagementProcessingService
    {
        private readonly ICloudManagementService cloudManagementService;
        private readonly IConfigurationBroker configurationBroker;

        public CloudManagementProcessingService()
        {
            this.cloudManagementService = new CloudManagementService();
            this.configurationBroker = new ConfigurationBroker();
        }

        public async ValueTask ProcessAsync()
        {
            CloudManagementConfiguration cloudManagementConfiguration =
               this.configurationBroker.GetConfigurations();

            await ProvisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Up);

            await DeprovisionAsync(
                projectName: cloudManagementConfiguration.ProjectName,
                cloudAction: cloudManagementConfiguration.Down);
        }

        private async ValueTask ProvisionAsync(
           string projectName,
           CloudAction cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                IResourceGroup resourceGroup = await this.cloudManagementService
                    .ProvisionResourceGroupAsync(
                        projectName,
                        environmentName);

                IAppServicePlan appServicePlan = await this.cloudManagementService
                    .ProvisionPlanAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                ISqlServer sqlServer = await this.cloudManagementService
                    .ProvisionSqlServerAsync(
                        projectName,
                        environmentName,
                        resourceGroup);

                SqlDatabase sqlDatabase = await this.cloudManagementService
                    .ProvisionSqlDatabaseAsync(
                        projectName,
                        environmentName,
                        sqlServer);

                IWebApp webApp = await this.cloudManagementService
                    .ProvisionWebAppAsync(
                        projectName,
                        environmentName,
                        sqlDatabase.ConnectionString,
                        resourceGroup,
                        appServicePlan);
            }
        }

        private async ValueTask DeprovisionAsync(
            string projectName,
            CloudAction cloudAction)
        {
            List<string> environments = RetrieveEnvironments(cloudAction);

            foreach (string environmentName in environments)
            {
                await this.cloudManagementService.DeprovisionResouceGroupAsync(
                    projectName,
                    environmentName);
            }
        }

        private static List<string> RetrieveEnvironments(CloudAction cloudAction) =>
            cloudAction?.Environments ?? new List<string>();
    }
}
