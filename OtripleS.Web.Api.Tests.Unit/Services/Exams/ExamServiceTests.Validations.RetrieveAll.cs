// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenExamsWasEmptyAndLogIt()
        {
            // given
            IQueryable<Exam> emptyStorageExams = new List<Exam>().AsQueryable();
            IQueryable<Exam> expectedExams = emptyStorageExams;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExams())
                    .Returns(expectedExams);

            // when
            IQueryable<Exam> actualExams =
                this.examService.RetrieveAllExams();

            // then
            actualExams.Should().BeEquivalentTo(emptyStorageExams);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No exams found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExams(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
