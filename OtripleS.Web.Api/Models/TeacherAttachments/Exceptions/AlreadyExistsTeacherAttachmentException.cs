﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.TeacherAttachments.Exceptions
{
    public class AlreadyExistsTeacherAttachmentException : Exception
    {
        public AlreadyExistsTeacherAttachmentException(Exception innerException)
            : base(message: "Teacher attachment with the same id already exists.", innerException) { }
    }
}
