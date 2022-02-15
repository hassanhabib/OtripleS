// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;

namespace OtripleS.Web.Api.Infrastructure.Provision.Services.Proccesings.CloudManagements
{
    public interface ICloudManagementProcessingService
    {
        ValueTask ProcessAsync();
    }
}
