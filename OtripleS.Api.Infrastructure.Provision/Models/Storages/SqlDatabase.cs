// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using Microsoft.Azure.Management.Sql.Fluent;

namespace OtripleS.Web.Api.Infrastructure.Provision.Models.Storages
{
    public class SqlDatabase
    {
        public string ConnectionString { get; set; }
        public ISqlDatabase Database { get; set; }
    }
}
