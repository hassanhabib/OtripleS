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
        public async void ShouldThrowValidationExceptionOnAddWhenStudentRegistrationIsNullAndLogItAsync()
        {
            // given
            StudentRegistration nullStudentRegistration = default;
            var nullStudentRegistrationException = new NullStudentRegistrationException();

            var expectedStudentRegistrationValidationException =
                new StudentRegistrationValidationException(nullStudentRegistrationException);

            // when
            ValueTask<StudentRegistration> addStudentRegistrationTask =
                this.studentRegistrationService.AddStudentRegistrationAsync(nullStudentRegistration);

            // then
            await Assert.ThrowsAsync<StudentRegistrationValidationException>(() =>
                addStudentRegistrationTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentRegistrationValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentRegistrationAsync(It.IsAny<StudentRegistration>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
