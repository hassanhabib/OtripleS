// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;
using OtripleS.Web.Api.Models.StudentRegistrations;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
    }
}
