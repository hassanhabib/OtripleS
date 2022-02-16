﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentGuardians.Exceptions
{
    public class NotFoundStudentGuardianException : Exception
    {
        public NotFoundStudentGuardianException(Guid studentId, Guid guardianId)
            : base(message: $"Couldn't find student guardian with student id: {studentId} " +
               $"and guardian: {guardianId}.")
        { }
    }
}
