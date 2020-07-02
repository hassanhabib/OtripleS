namespace OtripleS.Web.Api.Utils
{
    public static class GenericExtensions
    {
        public static bool HasValue(this object? item)
        {
            if (item is string s) return !string.IsNullOrEmpty(s);
            return item != null;
        }
    }
}