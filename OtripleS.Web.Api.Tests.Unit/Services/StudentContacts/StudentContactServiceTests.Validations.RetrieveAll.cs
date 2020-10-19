//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenStudentContactsWereEmptyAndLogIt()
        {
            // given
            IQueryable<StudentContact> emptyStorageStudentContacts =
                new List<StudentContact>().AsQueryable();

            IQueryable<StudentContact> expectedStudentContacts =
                emptyStorageStudentContacts;

            string expectedMessage = "No studentContacts found in storage.";

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentContacts())
                    .Returns(expectedStudentContacts);

            // when
            IQueryable<StudentContact> actualSemesterCourses =
                this.studentContactService.RetrieveAllStudentContacts();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(emptyStorageStudentContacts);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(expectedMessage),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentContacts(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
