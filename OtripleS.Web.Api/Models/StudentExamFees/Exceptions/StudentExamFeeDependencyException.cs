//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;

namespace OtripleS.Web.Api.Models.StudentExamFees.Exceptions
{
    public class StudentExamFeeDependencyException : Exception
    {
<<<<<<< HEAD
        public StudentExamFeeDependencyException(Exception innerException)
            : base("Service dependency error occurred, contact support.", innerException) { }
=======
        public StudentExamFeeDependencyException(Exception innerException) :
            base("Service dependency error occurred, contact support.", innerException)
        { }
>>>>>>> ef731125589f73b5a7c937a68dc7df752e17ae8c
    }
}
