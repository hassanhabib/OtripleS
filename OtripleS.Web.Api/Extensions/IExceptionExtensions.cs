// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Extensions
{
    public static class IExceptionExtensions
    {
        /// <summary>
        /// Get Inner Exception Message for the Exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static string GetInnerMessage(this Exception exception) => exception.InnerException.Message;
    }
}
