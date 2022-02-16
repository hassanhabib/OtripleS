﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenRegistrationIsNullAndLogItAsync()
        {
            //given
            Registration invalidRegistration = null;
            var nullRegistrationException = new NullRegistrationException();

            var expectedRegistrationValidationException =
                new RegistrationValidationException(nullRegistrationException);

            //when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            //then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenRegistrationIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidRegistrationId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.Id = invalidRegistrationId;

            var invalidRegistrationException = new InvalidRegistrationException(
                parameterName: nameof(Registration.Id),
                parameterValue: invalidRegistration.Id);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationException);

            //when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            //then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentNameIsInvalidAndLogItAsync(
            string invalidRegistrationStudentName)
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.StudentName = invalidRegistrationStudentName;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.StudentName),
                parameterValue: invalidRegistration.StudentName);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidEmailAddressCases))]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentEmailIsInvalidAndLogItAsync(
            string invalidRegistrationStudentEmail)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.StudentEmail = invalidRegistrationStudentEmail;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.StudentEmail),
                parameterValue: invalidRegistration.StudentEmail);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("b1234567890")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenStudentPhoneIsInvalidAndLogItAsync(
            string invalidRegistrationStudentPhone)
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.StudentPhone = invalidRegistrationStudentPhone;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.StudentPhone),
                parameterValue: invalidRegistration.StudentPhone);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenSubmitterNameIsInvalidAndLogItAsync(
            string invalidRegistrationSubmitterName)
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterName = invalidRegistrationSubmitterName;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterName),
               parameterValue: invalidRegistration.SubmitterName);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidEmailAddressCases))]
        public async Task ShouldThrowValidationExceptionOnModifyWhenSubmitterEmailIsInvalidAndLogItAsync(
            string invalidRegistrationSubmitterEmail)
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterEmail = invalidRegistrationSubmitterEmail;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterEmail),
               parameterValue: invalidRegistration.SubmitterEmail);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("b1234567890")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenSubmitterPhoneIsInvalidAndLogItAsync(
            string invalidRegistrationSubmitterPhone)
        {
            // given
            DateTimeOffset datetime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterPhone = invalidRegistrationSubmitterPhone;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterPhone),
               parameterValue: invalidRegistration.SubmitterPhone);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedBy = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedBy),
                parameterValue: inputRegistration.CreatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedBy),
                parameterValue: inputRegistration.UpdatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedDate = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedDate),
                parameterValue: inputRegistration.CreatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modoifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modoifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedDate = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedDate),
                parameterValue: inputRegistration.UpdatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedDate),
                parameterValue: inputRegistration.UpdatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfRegistrationDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration nonExistentRegistration = randomRegistration;
            nonExistentRegistration.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            Registration noRegistration = null;
            var notFoundRegistrationException = new NotFoundRegistrationException(nonExistentRegistration.Id);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(notFoundRegistrationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(nonExistentRegistration.Id))
                    .ReturnsAsync(noRegistration);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(nonExistentRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(nonExistentRegistration.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void
            ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsNotSameAsStorageCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            DateTimeOffset randomOtherDateTime = GetRandomDateTime();
            int randomNumber = GetRandomNumber();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedDate = dateTime.AddMinutes(randomNumber * -1);
            Registration storageRegistration = randomRegistration.DeepClone();
            storageRegistration.CreatedDate = randomOtherDateTime;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedDate),
                parameterValue: inputRegistration.CreatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistration.Id))
                    .ReturnsAsync(storageRegistration);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void
            ShouldThrowValidationExceptionOnModifyWhenCreatedByIsNotSameAsStorageCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guid randomCreatedById = Guid.NewGuid();
            int randomNumber = GetRandomNumber();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedDate = dateTime.AddMinutes(randomNumber * -1);
            Registration storageRegistration = randomRegistration.DeepClone();
            storageRegistration.CreatedBy = randomCreatedById;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedBy),
                parameterValue: inputRegistration.CreatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistration.Id))
                    .ReturnsAsync(storageRegistration);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void
            ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsSameAsStorageUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Guid randomCreatedById = Guid.NewGuid();
            int randomNumber = GetRandomNumber();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.CreatedDate = dateTime.AddMinutes(randomNumber * -1);
            Registration storageRegistration = randomRegistration.DeepClone();

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedDate),
                parameterValue: inputRegistration.UpdatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(inputRegistration.Id))
                    .ReturnsAsync(storageRegistration);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(It.IsAny<Guid>()),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedRegistrationValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationOnModifyIfForeignKeyConstraintConflictExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration storageRegistration = randomRegistration;
            Registration foreignKeyConflictedRegistration = storageRegistration.DeepClone();
            storageRegistration.UpdatedDate = dateTime.AddMinutes(GetNegativeRandomNumber());
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;

            var foreignKeyConstraintConflictException =
                new ForeignKeyConstraintConflictException(exceptionMessage);

            var invalidRegistrationReferenceException =
                new InvalidRegistrationReferenceException(foreignKeyConstraintConflictException);

            var registrationValidationException =
                new RegistrationValidationException(invalidRegistrationReferenceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectRegistrationByIdAsync(foreignKeyConflictedRegistration.Id))
                    .ReturnsAsync(storageRegistration);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateRegistrationAsync(foreignKeyConflictedRegistration))
                    .ThrowsAsync(foreignKeyConstraintConflictException);

            // when
            ValueTask<Registration> modifyRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(foreignKeyConflictedRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                modifyRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectRegistrationByIdAsync(foreignKeyConflictedRegistration.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateRegistrationAsync(foreignKeyConflictedRegistration),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    registrationValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
