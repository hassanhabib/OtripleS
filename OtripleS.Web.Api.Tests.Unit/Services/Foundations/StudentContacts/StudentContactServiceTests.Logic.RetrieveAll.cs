// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudentContacts()
        {
            // given
            IQueryable<StudentContact> randomSemesterCourses = CreateRandomStudentContacts();
            IQueryable<StudentContact> storageStudentContacts = randomSemesterCourses;
            IQueryable<StudentContact> expectedStudentContacts = storageStudentContacts;

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentContacts())
                .Returns(storageStudentContacts);

            // when
            IQueryable<StudentContact> actualStudentContacts =
                this.studentContactService.RetrieveAllStudentContacts();

            actualStudentContacts.Should().BeEquivalentTo(expectedStudentContacts);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentContacts(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
