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

        
    }
}
