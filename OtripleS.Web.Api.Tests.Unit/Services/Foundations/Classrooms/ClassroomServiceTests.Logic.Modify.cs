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
using OtripleS.Web.Api.Models.Classrooms;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldModifyClassroomAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(randomInputDate);
            Classroom inputClassroom = randomClassroom;
            Classroom afterUpdateStorageClassroom = inputClassroom;
            Classroom expectedClassroom = afterUpdateStorageClassroom;
            Classroom beforeUpdateStorageClassroom = randomClassroom.DeepClone();
            inputClassroom.UpdatedDate = randomDate;
            Guid classroomId = inputClassroom.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(classroomId))
                    .ReturnsAsync(beforeUpdateStorageClassroom);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateClassroomAsync(inputClassroom))
                    .ReturnsAsync(afterUpdateStorageClassroom);

            // when
            Classroom actualClassroom =
                await this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            actualClassroom.Should().BeEquivalentTo(expectedClassroom);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(classroomId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateClassroomAsync(inputClassroom),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
