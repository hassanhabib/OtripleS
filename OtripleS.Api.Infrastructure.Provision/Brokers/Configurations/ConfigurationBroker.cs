// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.IO;
using Microsoft.Extensions.Configuration;
using OtripleS.Web.Api.Infrastructure.Provision.Models.Configurations;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Configurations
{
    public class ConfigurationBroker : IConfigurationBroker
    {
        public CloudManagementConfiguration GetConfigurations()
        {
            IConfigurationRoot configurationRoot = new ConfigurationBuilder()
               .SetBasePath(basePath: Directory.GetCurrentDirectory())
               .AddJsonFile(path: "appSettings.json", optional: false)
               .Build();

            return configurationRoot.Get<CloudManagementConfiguration>();
        }
    }
}
