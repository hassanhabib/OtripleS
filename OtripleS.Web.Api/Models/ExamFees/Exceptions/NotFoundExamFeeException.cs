﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.ExamFees.Exceptions
{
    public class NotFoundExamFeeException : Exception
    {
        public NotFoundExamFeeException(Guid examFeeId)
            : base(message: $"Couldn't find exam fee with id: {examFeeId}.") { }
    }
}
