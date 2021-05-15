// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Registrations;
using OtripleS.Web.Api.Models.Registrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Registrations
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.AddRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
            ValueTask<Registration> registerRegistrationTask =
                this.registrationService.ModifyRegistrationAsync(invalidRegistration);

            // then
            await Assert.ThrowsAsync<RegistrationValidationException>(() =>
                registerRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
                broker.LogError(It.Is(SameExceptionAs(expectedRegistrationValidationException))),
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
    }
}
