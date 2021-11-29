// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using Xeptions;

namespace OtripleS.Web.Api.Models.Classrooms.Exceptions
{
    public class LockedClassroomException : Xeption
    {
        public LockedClassroomException(Exception innerException)
            : base(message: "Locked classroom record exception, please try again later.", innerException) { }
    }
}