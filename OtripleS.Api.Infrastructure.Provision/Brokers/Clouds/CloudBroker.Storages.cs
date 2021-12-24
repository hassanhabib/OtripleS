using System.Threading.Tasks;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.Azure.Management.Sql.Fluent;
using OtripleS.Web.Api.Infrastructure.Provision.Models.Storages;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Clouds
{
    public partial class CloudBroker
    {
        public async ValueTask<ISqlServer> CreateSqlServerAsync(
           string sqlServerName,
           IResourceGroup resourceGroup)
        {
            return await this.azure.SqlServers
                .Define(sqlServerName)
                .WithRegion(Region.USWest2)
                .WithExistingResourceGroup(resourceGroup)
                .WithAdministratorLogin(this.adminName)
                .WithAdministratorPassword(this.adminAccess)
                .CreateAsync();
        }

        public async ValueTask<ISqlDatabase> CreateSqlDatabaseAsync(
            string sqlDatabaseName,
            ISqlServer sqlServer)
        {
            return await this.azure.SqlServers.Databases
                .Define(sqlDatabaseName)
                .WithExistingSqlServer(sqlServer)
                .CreateAsync();
        }

        public SqlDatabaseAccess GetAdminAccess()
        {
            return new SqlDatabaseAccess
            {
                AdminName = this.adminName,
                AdminAccess = this.adminAccess
            };
        }
    }
}
