// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using OtripleS.Web.Api.Models.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentExamFeeId = Guid.NewGuid();
            Guid inputStudentExamFeeId = randomStudentExamFeeId;
            var sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExamFee> retrieveStudentExamFeeByIdTask =
                this.studentExamFeeService.RetrieveStudentExamFeeByIdAsync(inputStudentExamFeeId);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                retrieveStudentExamFeeByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdAsync(inputStudentExamFeeId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
