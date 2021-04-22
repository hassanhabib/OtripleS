//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class LockedStudentExamFeeException : Exception
    {
        public LockedStudentExamFeeException(Exception innerException)
          : base("Locked Assignment Attachment record exception, please try again later.", innerException) { }
    }
}
