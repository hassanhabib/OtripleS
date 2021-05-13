using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
{
    public partial class RegistrationServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRegistrationIsNullAndLogItAsync()
        {
            // given
            Registration randomRegistration = default;
            Registration nullRegistration = randomRegistration;
            var nullRegistrationException = new NullRegistrationException();

            var expectedRegistrationValidationException =
                new RegistrationValidationException(nullRegistrationException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(nullRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenStudentNameIsInvalidAndLogItAsync(
            string invalidRegistrationStudentName)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.StudentName = invalidRegistrationStudentName;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.StudentName),
               parameterValue: invalidRegistration.StudentName);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenSubmitterNameIsInvalidAndLogItAsync(
            string invalidRegistrationSubmitterName)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterName = invalidRegistrationSubmitterName;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterName),
               parameterValue: invalidRegistration.SubmitterName);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenSubmitterEmailIsInvalidAndLogItAsync(
    string invalidRegistrationSubmitterEmail)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterEmail = invalidRegistrationSubmitterEmail;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterEmail),
               parameterValue: invalidRegistration.SubmitterEmail);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenSubmitterPhoneIsInvalidAndLogItAsync(
            string invalidRegistrationSubmitterPhone)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Registration randomRegistration = CreateRandomRegistration(datetime);
            Registration invalidRegistration = randomRegistration;
            invalidRegistration.SubmitterPhone = invalidRegistrationSubmitterPhone;

            var invalidRegistrationInputException = new InvalidRegistrationException(
               parameterName: nameof(Registration.SubmitterPhone),
               parameterValue: invalidRegistration.SubmitterPhone);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }


        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.Id = default;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.Id),
                parameterValue: inputRegistration.Id);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> createRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                createRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = Guid.NewGuid();

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedBy),
                parameterValue: inputRegistration.UpdatedBy);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = randomRegistration.CreatedBy;
            inputRegistration.UpdatedDate = GetRandomDateTime();

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.UpdatedDate),
                parameterValue: inputRegistration.UpdatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration inputRegistration = randomRegistration;
            inputRegistration.UpdatedBy = inputRegistration.CreatedBy;
            inputRegistration.CreatedDate = dateTime.AddMinutes(minutes);
            inputRegistration.UpdatedDate = inputRegistration.CreatedDate;

            var invalidRegistrationInputException = new InvalidRegistrationException(
                parameterName: nameof(Registration.CreatedDate),
                parameterValue: inputRegistration.CreatedDate);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(invalidRegistrationInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(inputRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(It.IsAny<Registration>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenRegistrationAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Registration randomRegistration = CreateRandomRegistration(dateTime);
            Registration alreadyExistsRegistration = randomRegistration;
            alreadyExistsRegistration.UpdatedBy = alreadyExistsRegistration.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsRegistrationException =
                new AlreadyExistsRegistrationException(duplicateKeyException);

            var expectedRegistrationValidationException =
                new RegistrationValidationException(alreadyExistsRegistrationException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertRegistrationAsync(alreadyExistsRegistration))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(alreadyExistsRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertRegistrationAsync(alreadyExistsRegistration),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

    }
}
