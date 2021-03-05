//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Models.AssignmentAttachments.Exceptions
{
    public class LockedAssignmentAttachmentException : Exception
    {
        public LockedAssignmentAttachmentException(Exception innerException)
          : base("Locked Assignment Attachment record exception, please try again later.", innerException) { }
    }
}
