﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogActivity(string message);
    }
}
