//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

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
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = inputStudentExamFee.CreatedBy;
            inputStudentExamFee.UpdatedDate = inputStudentExamFee.CreatedDate;
            var sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = inputStudentExamFee.CreatedBy;
            inputStudentExamFee.UpdatedDate = inputStudentExamFee.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee(dateTime);
            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            inputStudentExamFee.UpdatedBy = inputStudentExamFee.CreatedBy;
            inputStudentExamFee.UpdatedDate = inputStudentExamFee.CreatedDate;
            var exception = new Exception();

            var expectedStudentExamFeeServiceException =
                new StudentExamFeeServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(inputStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeServiceException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
