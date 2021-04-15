// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldAddExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = DateTimeOffset.UtcNow;
            ExamFee randomExamFee = CreateRandomExamFee();
            randomExamFee.UpdatedBy = randomExamFee.CreatedBy;
            randomExamFee.UpdatedDate = randomExamFee.CreatedDate;
            ExamFee inputExamFee = randomExamFee;
            ExamFee storageExamFee = randomExamFee;
            ExamFee expectedExamFee = randomExamFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertExamFeeAsync(inputExamFee))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.AddExamFeeAsync(inputExamFee);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamFeeAsync(inputExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllExamFees()
        {
            // given
            IQueryable<ExamFee> randomExamFees =
                CreateRandomExamFees();

            IQueryable<ExamFee> storageExamFees =
                randomExamFees;

            IQueryable<ExamFee> expectedExamFees =
                storageExamFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExamFees())
                    .Returns(storageExamFees);

            // when
            IQueryable<ExamFee> actualExamFees =
                this.examFeeService.RetrieveAllExamFees();

            // then
            actualExamFees.Should().BeEquivalentTo(expectedExamFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExamFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
