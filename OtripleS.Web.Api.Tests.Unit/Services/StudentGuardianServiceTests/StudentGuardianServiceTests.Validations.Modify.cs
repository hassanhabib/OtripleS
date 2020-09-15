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
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
{
	public partial class StudentGuardianServiceTests
	{
        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenStudentGuardianIsNullAndLogItAsync()
        {
            //given
            StudentGuardian invalidStudentGuardian = null;
            var nullStudentGuardianException = new NullStudentGuardianException();

            var expectedSemesterCourseValidationException =
                new StudentGuardianValidationException(nullStudentGuardianException);

            //when
            ValueTask<StudentGuardian> modifyStudentGuardianTask =
                this.studentGuardianService.ModifyStudentGuardianAsync(invalidStudentGuardian);

            //then
            await Assert.ThrowsAsync<StudentGuardianValidationException>(() =>
                modifyStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
