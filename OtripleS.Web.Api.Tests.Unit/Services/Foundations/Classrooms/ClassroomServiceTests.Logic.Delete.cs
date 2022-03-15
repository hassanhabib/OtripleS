// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldDeleteClassroomAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dates: dateTime);
            Guid inputClassroomId = randomClassroom.Id;
            Classroom inputClassroom = randomClassroom;
            Classroom storageClassroom = randomClassroom;
            Classroom expectedClassroom = randomClassroom;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ReturnsAsync(inputClassroom);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteClassroomAsync(inputClassroom))
                    .ReturnsAsync(storageClassroom);

            // when
            Classroom actualClassroom =
                await this.classroomService.RemoveClassroomAsync(inputClassroomId);

            // then
            actualClassroom.Should().BeEquivalentTo(expectedClassroom);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteClassroomAsync(inputClassroom),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
