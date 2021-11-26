// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using OtripleS.Web.Api.Infrastructure.Provision.Models.Configurations;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Configurations
{
    public interface IConfigurationBroker
    {
        CloudManagementConfiguration GetConfigurations();
    }
}
