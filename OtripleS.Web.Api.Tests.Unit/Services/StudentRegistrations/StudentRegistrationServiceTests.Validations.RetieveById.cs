// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = default;
            Guid inputStudentId = randomStudentId;
            Guid randomRegistrationId = default;
            Guid inputRegistrationId = randomRegistrationId;
            var invalidStudentRegistrationInputException = new InvalidStudentRegistrationInputException(
                parameterName: nameof(StudentRegistration.StudentId),
                parameterValue: inputStudentId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationInputException);

            // when
            ValueTask<StudentRegistration> retrieveStudentRegistrationByIdTask =
                this.studentRegistrationService.RetrieveStudentRegistrationByIdAsync(inputStudentId , inputRegistrationId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                retrieveStudentRegistrationByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
