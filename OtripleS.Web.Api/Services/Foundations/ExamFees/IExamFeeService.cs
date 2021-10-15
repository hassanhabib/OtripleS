//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using OtripleS.Web.Api.Models.ExamFees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.ExamFees
{
    public interface IExamFeeService
    {
        ValueTask<ExamFee> AddExamFeeAsync(ExamFee examFee);
        IQueryable<ExamFee> RetrieveAllExamFees();
        ValueTask<ExamFee> RetrieveExamFeeByIdAsync(Guid examFeeId);
        ValueTask<ExamFee> ModifyExamFeeAsync(ExamFee examFee);
        ValueTask<ExamFee> RemoveExamFeeByIdAsync(Guid examFeeId);
    }
}
