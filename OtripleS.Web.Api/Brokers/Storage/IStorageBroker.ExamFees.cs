// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<ExamFee> InsertExamFeeAsync(ExamFee examFee);
        public IQueryable<ExamFee> SelectAllExamFees();
        public ValueTask<ExamFee> SelectExamFeeByIdAsync(Guid id);
        public ValueTask<ExamFee> UpdateExamFeeAsync(ExamFee examFee);
        public ValueTask<ExamFee> DeleteExamFeeAsync(ExamFee examFee);
    }
}
