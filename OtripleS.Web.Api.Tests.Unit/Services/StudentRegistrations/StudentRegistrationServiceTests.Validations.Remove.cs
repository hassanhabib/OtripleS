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
        public async Task ShouldThrowValidationExceptionOnDeleteWhenStudentRegistrationIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentRegistrationId = default;
            Guid randomStudentId = default;
            Guid inputStudentRegistrationId = randomStudentRegistrationId;
            Guid inputStudentId = randomStudentId;

            var invalidStudentRegistrationInputException = new InvalidStudentRegistrationInputException(
                parameterName: nameof(StudentRegistration.RegistrationId),
                parameterValue: inputStudentRegistrationId);

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(invalidStudentRegistrationInputException);

            //when
            ValueTask<StudentRegistration> actualStudentRegistrationDeleteTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputStudentRegistrationId, 
                    inputStudentId);

            //then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(
                () => actualStudentRegistrationDeleteTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();            
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
        
    }
}
