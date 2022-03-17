// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Services.Foundations.StudentExamFees
{
    public interface IStudentExamFeeService
    {
        ValueTask<StudentExamFee> AddStudentExamFeeAsync(StudentExamFee studentExamFee);
        IQueryable<StudentExamFee> RetrieveAllStudentExamFees();
        ValueTask<StudentExamFee> RetrieveStudentExamFeeByIdsAsync( Guid studentId, Guid examFeeId);
        ValueTask<StudentExamFee> ModifyStudentExamFeeAsync(StudentExamFee studentExamFee);
        ValueTask<StudentExamFee> RemoveStudentExamFeeByIdAsync(Guid studentId, Guid examFeeId);
    }
}