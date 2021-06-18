//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees;
using OtripleS.Web.Api.Models.Foundations.StudentExamFees.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee someStudentExamFee = CreateRandomStudentExamFee(dateTime);
            someStudentExamFee.UpdatedBy = someStudentExamFee.CreatedBy;
            someStudentExamFee.UpdatedDate = someStudentExamFee.CreatedDate;
            var sqlException = GetSqlException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(someStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee someStudentExamFee = CreateRandomStudentExamFee(dateTime);
            someStudentExamFee.UpdatedBy = someStudentExamFee.CreatedBy;
            someStudentExamFee.UpdatedDate = someStudentExamFee.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentExamFeeDependencyException =
                new StudentExamFeeDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(someStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeDependencyException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExamFee someStudentExamFee = CreateRandomStudentExamFee(dateTime);
            someStudentExamFee.UpdatedBy = someStudentExamFee.CreatedBy;
            someStudentExamFee.UpdatedDate = someStudentExamFee.CreatedDate;
            var exception = new Exception();

            var expectedStudentExamFeeServiceException =
                new StudentExamFeeServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentExamFee> createStudentExamFeeTask =
                this.studentExamFeeService.AddStudentExamFeeAsync(someStudentExamFee);

            // then
            await Assert.ThrowsAsync<StudentExamFeeServiceException>(() =>
                createStudentExamFeeTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamFeeServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamFeeAsync(It.IsAny<StudentExamFee>()),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
