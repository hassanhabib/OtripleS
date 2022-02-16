﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentExamFee> InsertStudentExamFeeAsync(StudentExamFee studentExamFee);
        IQueryable<StudentExamFee> SelectAllStudentExamFees();

        ValueTask<StudentExamFee> SelectStudentExamFeeByIdsAsync(
            Guid studentId,
            Guid examFeeId);

        ValueTask<StudentExamFee> UpdateStudentExamFeeAsync(StudentExamFee studentExamFee);
        ValueTask<StudentExamFee> DeleteStudentExamFeeAsync(StudentExamFee studentExamFee);
    }
}