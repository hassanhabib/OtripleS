// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.StudentExamFees;

namespace OtripleS.Web.Api.Services.StudentExamFees
{
    public partial class StudentExamFeeService : IStudentExamFeeService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public StudentExamFeeService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<StudentExamFee> RemoveStudentExamFeeByIdAsync(
            Guid studentExamFeeId) =>
            TryCatch(async () => 
            {
                ValidateStudentExamFeeId(studentExamFeeId);
                
                StudentExamFee maybeStudentExamFee =
                    await this.storageBroker.SelectStudentExamFeeByIdAsync(studentExamFeeId);

                return await this.storageBroker.DeleteStudentExamFeeAsync(maybeStudentExamFee);
            });
    }
}
