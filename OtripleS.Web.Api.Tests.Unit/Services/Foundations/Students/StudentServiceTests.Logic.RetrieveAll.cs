// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Students;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Students
{
    public partial class StudentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudents()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Student> randomStudents = CreateRandomStudents(randomDateTime);
            IQueryable<Student> storageStudents = randomStudents;
            IQueryable<Student> expectedStudents = storageStudents;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudents())
                    .Returns(storageStudents);

            // when
            IQueryable<Student> actualStudents =
                this.studentService.RetrieveAllStudents();

            // then
            actualStudents.Should().BeEquivalentTo(expectedStudents);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudents(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
