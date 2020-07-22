// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
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
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenClassroomIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidClassroomId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Classroom randomClassroom = CreateRandomClassroom(dateTime);
			Classroom invalidClassroom = randomClassroom;
			invalidClassroom.Id = invalidClassroomId;

			var invalidClassroomException = new InvalidClassroomInputException(
				parameterName: nameof(Classroom.Id),
				parameterValue: invalidClassroom.Id);

			var expectedClassroomValidationException =
				new ClassroomValidationException(invalidClassroomException);

			//when
			ValueTask<Classroom> modifyClassroomTask =
				this.classroomService.ModifyClassroomAsync(invalidClassroom);

			//then
			await Assert.ThrowsAsync<ClassroomValidationException>(() =>
				modifyClassroomTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
