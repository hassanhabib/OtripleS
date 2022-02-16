﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenClassroomIsNullAndLogItAsync()
        {
            //given
            Classroom invalidClassroom = null;
            var nullClassroomException = new NullClassroomException();

            var expectedClassroomValidationException =
                new ClassroomValidationException(nullClassroomException);

            //when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            //then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnModifyIfClassroomIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidClassroom = new Classroom
            {
                Name = invalidText,
                Location = invalidText
            };

            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.Id),
                values: "Id is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.Name),
                values: "Text is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.Location),
                values: "Text is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.CreatedDate),
                values: "Date is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedDate),
                values: new string[] {
                    "Date is required",
                    $"Date is the same as {nameof(Classroom.CreatedDate)}"
                });

            invalidClassroomException.AddData(
                key: nameof(Classroom.CreatedBy),
                values: "Id is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedBy),
                values: "Id is required");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = inputClassroom.CreatedBy;
            inputClassroom.UpdatedDate = dateTime.AddMinutes(minutes);
            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedDate),
                values: $"Date is not recent");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfClassroomDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom nonExistentClassroom = randomClassroom;
            nonExistentClassroom.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Classroom noClassroom = null;
            var notFoundClassroomException = new NotFoundClassroomException(nonExistentClassroom.Id);

            var expectedClassroomValidationException =
                new ClassroomValidationException(notFoundClassroomException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(nonExistentClassroom.Id))
                    .ReturnsAsync(noClassroom);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(nonExistentClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(nonExistentClassroom.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(randomDate);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.UpdatedDate = randomDate;
            Classroom storageClassroom = randomClassroom.DeepClone();
            Guid classroomId = invalidClassroom.Id;
            invalidClassroom.CreatedDate = storageClassroom.CreatedDate.AddMinutes(randomNumber);

            var invalidClassroomInputException = new InvalidClassroomException(
                parameterName: nameof(Classroom.CreatedDate),
                parameterValue: invalidClassroom.CreatedDate);

            var expectedClassroomValidationException =
              new ClassroomValidationException(invalidClassroomInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(classroomId))
                    .ReturnsAsync(storageClassroom);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(invalidClassroom.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(randomDate);
            randomClassroom.CreatedDate = randomClassroom.CreatedDate.AddMinutes(minutesInThePast);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.UpdatedDate = randomDate;
            Classroom storageClassroom = randomClassroom.DeepClone();
            Guid classroomId = invalidClassroom.Id;

            var invalidClassroomInputException = new InvalidClassroomException(
                parameterName: nameof(Classroom.UpdatedDate),
                parameterValue: invalidClassroom.UpdatedDate);

            var expectedClassroomValidationException =
              new ClassroomValidationException(invalidClassroomInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(classroomId))
                    .ReturnsAsync(storageClassroom);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(invalidClassroom.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(randomDate);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            Classroom storageClassroom = randomClassroom.DeepClone();
            Guid classroomId = invalidClassroom.Id;
            invalidClassroom.CreatedBy = invalidCreatedBy;

            var invalidClassroomInputException = new InvalidClassroomException(
                parameterName: nameof(Classroom.CreatedBy),
                parameterValue: invalidClassroom.CreatedBy);

            var expectedClassroomValidationException =
              new ClassroomValidationException(invalidClassroomInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(classroomId))
                    .ReturnsAsync(storageClassroom);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(invalidClassroom.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
