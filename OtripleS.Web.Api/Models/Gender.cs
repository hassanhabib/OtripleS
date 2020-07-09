// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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