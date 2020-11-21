// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentExamsWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentExam> emptyStorageStudentExams = new List<StudentExam>().AsQueryable();
            IQueryable<StudentExam> expectedStudentExams = emptyStorageStudentExams;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExams())
                    .Returns(expectedStudentExams);

            // when
            IQueryable<StudentExam> actualStudentExams =
                this.studentExamService.RetrieveAllStudentExams();

            // then
            actualStudentExams.Should().BeEquivalentTo(emptyStorageStudentExams);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No Student Exams found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExams(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
