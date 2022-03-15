// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
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
