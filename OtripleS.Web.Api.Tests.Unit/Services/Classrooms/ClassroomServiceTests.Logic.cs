// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldCreateClassroomAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Classroom randomClassroom = CreateRandomClassroom(randomDateTime);
            randomClassroom.UpdatedBy = randomClassroom.CreatedBy;
            randomClassroom.UpdatedDate = randomClassroom.CreatedDate;
            Classroom inputClassroom = randomClassroom;
            Classroom storageClassroom = randomClassroom;
            Classroom expectedClassroom = storageClassroom;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClassroomAsync(inputClassroom))
                    .ReturnsAsync(storageClassroom);

            // when
            Classroom actualClassroom =
                await this.classroomService.CreateClassroomAsync(inputClassroom);

            // then
            actualClassroom.Should().BeEquivalentTo(expectedClassroom);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(inputClassroom),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

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
                await this.classroomService.DeleteClassroomAsync(inputClassroomId);

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

        [Fact]
        public void ShouldRetrieveAllClassrooms()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Classroom> randomClassrooms = CreateRandomClassrooms(randomDateTime);
            IQueryable<Classroom> storageClassrooms = randomClassrooms;
            IQueryable<Classroom> expectedClassrooms = storageClassrooms;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Returns(storageClassrooms);

            // when
            IQueryable<Classroom> actualClassrooms =
                this.classroomService.RetrieveAllClassrooms();

            // then
            actualClassrooms.Should().BeEquivalentTo(expectedClassrooms);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveClassroomById()
        {
            //given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Guid inputClassroomId = randomClassroom.Id;
            Classroom inputClassroom = randomClassroom;
            Classroom expectedClassroom = randomClassroom;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectClassroomByIdAsync(inputClassroomId))
                .ReturnsAsync(inputClassroom);

            //when 
            Classroom actualClassroom = await this.classroomService.RetrieveClassroomById(inputClassroomId);

            //then
            actualClassroom.Should().BeEquivalentTo(expectedClassroom);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId), Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
