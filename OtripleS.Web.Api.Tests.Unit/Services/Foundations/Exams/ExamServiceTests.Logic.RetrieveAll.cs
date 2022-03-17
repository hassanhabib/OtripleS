// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllExams()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Exam> randomExams = CreateRandomExams(randomDateTime);
            IQueryable<Exam> storageExams = randomExams;
            IQueryable<Exam> expectedExams = storageExams;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllExams())
                    .Returns(storageExams);

            // when
            IQueryable<Exam> actualExams =
                this.examService.RetrieveAllExams();

            // then
            actualExams.Should().BeEquivalentTo(expectedExams);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllExams(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
