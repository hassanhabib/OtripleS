using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
{
   public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldLogWarningOnRetrieveAllWhenClassroomWereEmptyAndLogIt()
        {
            // given
            IQueryable<Classroom> emptyStorageClassroom = new List<Classroom>().AsQueryable();
            IQueryable<Classroom> expectedClassroom = emptyStorageClassroom;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Returns(expectedClassroom);

            // when
            IQueryable<Classroom> actualClassroom =
                this.classroomService.RetrieveAllClassrooms();

            // then
            actualClassroom.Should().BeEquivalentTo(emptyStorageClassroom);

            this.loggingBrokerMock.Verify(broker =>
            broker.LogWarning("No Classroom found in storage."), 
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
