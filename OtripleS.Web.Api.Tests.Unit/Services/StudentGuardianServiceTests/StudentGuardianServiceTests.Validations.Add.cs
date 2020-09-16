// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
{
    public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenStudentGuardianIsNullAndLogItAsync()
        {
            // given
            StudentGuardian randomStudentGuardian = default;
            StudentGuardian nullStudentGuardian = randomStudentGuardian;
            var nullStudentGuardianException = new NullStudentGuardianException();

            var expectedStudentGuardianValidationException =
                new StudentGuardianValidationException(nullStudentGuardianException);

            // when
            ValueTask<StudentGuardian> createStudentGuardianTask =
                this.studentGuardianService.AddStudentGuardianAsync(nullStudentGuardian);

            // then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                createStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentGuardianAsync(It.IsAny<StudentGuardian>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}