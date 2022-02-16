﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.ExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<ExamFee> InsertExamFeeAsync(ExamFee examFee);
        IQueryable<ExamFee> SelectAllExamFees();
        ValueTask<ExamFee> SelectExamFeeByIdAsync(Guid examFeeId);
        ValueTask<ExamFee> UpdateExamFeeAsync(ExamFee examFee);
        ValueTask<ExamFee> DeleteExamFeeAsync(ExamFee examFee);
    }
}