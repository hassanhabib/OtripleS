// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using Microsoft.Azure.Management.Sql.Fluent;

namespace OtripleS.Web.Api.Infrastructure.Provision.Models.Storages
{
    public class SqlDatabase
    {
        public string ConnectionString { get; set; }
        public ISqlDatabase Database { get; set; }
    }
}
