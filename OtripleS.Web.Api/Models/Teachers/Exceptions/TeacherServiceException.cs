﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Teachers.Exceptions
{
    public class TeacherServiceException : Exception
    {
        public TeacherServiceException(Exception innerException)
            : base("Service error occurred, contact support.", innerException) { }
    }
}
