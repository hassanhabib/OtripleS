// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

namespace OtripleS.Web.Api.Infrastructure.Provision.Models.Configurations
{
    public class CloudManagementConfiguration
    {
        public string ProjectName { get; set; }
        public CloudAction Up { get; set; }
        public CloudAction Down { get; set; }
    }
}
