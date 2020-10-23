// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenClassroomsWasEmptyAndLogIt()
        {
            // given
            IQueryable<Classroom> emptyStorageClassrooms = new List<Classroom>().AsQueryable();
            IQueryable<Classroom> expectedClassrooms = emptyStorageClassrooms;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Returns(expectedClassrooms);

            // when
            IQueryable<Classroom> actualClassrooms =
                this.classroomService.RetrieveAllClassrooms();

            // then
            actualClassrooms.Should().BeEquivalentTo(emptyStorageClassrooms);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogWarning("No classrooms found in storage."),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
