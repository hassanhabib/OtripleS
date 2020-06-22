using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<OtripleSApiBroker>
    {
    }
}
