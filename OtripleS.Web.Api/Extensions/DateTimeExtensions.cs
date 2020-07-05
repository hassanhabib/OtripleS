using System;

namespace OtripleS.Web.Api.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsValid(this DateTimeOffset offset)
        {
            return offset.Date != DateTime.MinValue;
        }
        
    }
}