//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
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
        public void ShouldLogWarningOnRetrieveAllWhenExamFeesWereEmptyAndLogIt()
        {
            // given
            IQueryable<ExamFee> emptyStorageExamFees =
                new List<ExamFee>().AsQueryable();

            IQueryable<ExamFee> storageExamFees =
                emptyStorageExamFees;

            IQueryable<ExamFee> expectedExamFees =
                emptyStorageExamFees;

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

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No exam fees found in storage."),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

    }
}
