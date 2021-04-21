//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;

namespace OtripleS.Web.Api.Services.StudentExamFees
{
    public partial class StudentExamFeeService
    {
        private void ValidateStudentExamFeeOnCreate(StudentExamFee studentExamFee)
        {
            ValidateStudentExamFeeIsNull(studentExamFee);
        }

        private void ValidateStudentExamFeeIsNull(StudentExamFee studentExamFee)
        {
            if (studentExamFee is null)
            {
                throw new NullStudentExamFeeException();
            }
        }
    }
}
