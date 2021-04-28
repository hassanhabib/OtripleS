// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<StudentExamFee> InsertStudentExamFeeAsync(StudentExamFee examFee);
        public IQueryable<StudentExamFee> SelectAllStudentExamFees();
        public ValueTask<StudentExamFee> SelectStudentExamFeeByIdAsync(Guid id);
        public ValueTask<StudentExamFee> UpdateStudentExamFeeAsync(StudentExamFee examFee);
        public ValueTask<StudentExamFee> DeleteStudentExamFeeAsync(StudentExamFee examFee);
    }
}
