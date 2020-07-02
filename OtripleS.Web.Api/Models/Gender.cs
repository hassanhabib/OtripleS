namespace OtripleS.Web.Api.Models
{
    public enum Gender
    {
        Female,
        Male,
        Other,
        Unknown
    }

    public static class GenderHelper
    {
        public static Gender StringToGenderConverter(string gender)
        {
            return gender.ToLower() switch
            {
                "female" => Gender.Female,
                "male" => Gender.Male,
                "other" => Gender.Other,
                _ => Gender.Unknown
            };
        }
    }
}