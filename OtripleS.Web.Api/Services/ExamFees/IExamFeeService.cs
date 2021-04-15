//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Services.ExamFees
{
    public interface IExamFeeService
    {
        ValueTask<ExamFee> AddExamFeeAsync(ExamFee examFee);
        IQueryable<ExamFee> RetrieveAllExamFees();
    }
}
