//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenTeacherContactsWereEmptyAndLogIt()
        {
            // given
            IQueryable<TeacherContact> emptyStorageTeacherContacts =
                new List<TeacherContact>().AsQueryable();

            IQueryable<TeacherContact> expectedTeacherContacts =
                emptyStorageTeacherContacts;

            string expectedMessage = "No teacherContacts found in storage.";

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllTeacherContacts())
                    .Returns(expectedTeacherContacts);

            // when
            IQueryable<TeacherContact> actualSemesterCourses =
                this.teacherContactService.RetrieveAllTeacherContacts();

            // then
            actualSemesterCourses.Should().BeEquivalentTo(emptyStorageTeacherContacts);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning(expectedMessage),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllTeacherContacts(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
