// -------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE FOR THE WORLD
// -------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Infrastructure.Provision.Brokers.Loggings
{
    public class LoggingBroker : ILoggingBroker
    {
        public void LogActivity(string message) =>
            Console.WriteLine(message);
    }
}
