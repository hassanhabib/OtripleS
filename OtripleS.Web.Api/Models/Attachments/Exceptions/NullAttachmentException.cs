﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.Attachments.Exceptions
{
    public class NullAttachmentException : Exception
    {
        public NullAttachmentException() : base(message: "The attachment is null.") { }
    }
}
