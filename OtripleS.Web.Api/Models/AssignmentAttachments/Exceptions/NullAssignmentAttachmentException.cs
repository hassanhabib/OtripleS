﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class NullAssignmentAttachmentException : Exception
    {
        public NullAssignmentAttachmentException()
            : base(message: "The assignment attachment is null.") { }
    }
}
