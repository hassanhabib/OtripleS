// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.ExamFees;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ExamFee> InsertExamFeeAsync(ExamFee examFee);
        IQueryable<ExamFee> SelectAllExamFees();
        ValueTask<ExamFee> SelectExamFeeByIdAsync(Guid id);
        ValueTask<ExamFee> UpdateExamFeeAsync(ExamFee examFee);
        ValueTask<ExamFee> DeleteExamFeeAsync(ExamFee examFee);
    }
}