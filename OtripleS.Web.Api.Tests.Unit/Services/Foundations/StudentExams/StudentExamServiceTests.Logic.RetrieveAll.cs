// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudentExams()
        {
            // given
            IQueryable<StudentExam> randomStudentExams =
                CreateRandomStudentExams();

            IQueryable<StudentExam> storageStudentExams = randomStudentExams;
            IQueryable<StudentExam> expectedStudentExams = storageStudentExams;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentExams())
                    .Returns(storageStudentExams);

            // when
            IQueryable<StudentExam> actualStudentExams =
                this.studentExamService.RetrieveAllStudentExams();

            // then
            actualStudentExams.Should().BeEquivalentTo(expectedStudentExams);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentExams(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
