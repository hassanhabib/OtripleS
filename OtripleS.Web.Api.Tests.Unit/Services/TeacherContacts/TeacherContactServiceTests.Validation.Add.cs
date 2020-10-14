//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
	public partial class TeacherContactServiceTests
	{
		[Fact]
		public async void ShouldThrowValidationExceptionOnAddWhenTeacherContactIsNullAndLogItAsync()
		{
			// given
			TeacherContact randomTeacherContact = default;
			TeacherContact nullTeacherContact = randomTeacherContact;
			var nullTeacherContactException = new NullTeacherContactException();

			var expectedTeacherContactValidationException =
				new TeacherContactValidationException(nullTeacherContactException);

			// when
			ValueTask<TeacherContact> addTeacherContactTask =
				this.teacherContactService.AddTeacherContactAsync(nullTeacherContact);

			// then
			await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
				addTeacherContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertTeacherContactAsync(It.IsAny<TeacherContact>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async void ShouldThrowValidationExceptionOnAddWhenTeacherIdIsInvalidAndLogItAsync()
		{
			// given
			TeacherContact randomTeacherContact = CreateRandomTeacherContact();
			TeacherContact inputTeacherContact = randomTeacherContact;
			inputTeacherContact.TeacherId = default;

			var invalidTeacherContactInputException = new InvalidTeacherContactInputException(
				parameterName: nameof(TeacherContact.TeacherId),
				parameterValue: inputTeacherContact.TeacherId);

			var expectedTeacherContactValidationException =
				new TeacherContactValidationException(invalidTeacherContactInputException);

			// when
			ValueTask<TeacherContact> addTeacherContactTask =
				this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

			// then
			await Assert.ThrowsAsync<TeacherContactValidationException>(() =>
				addTeacherContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedTeacherContactValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.InsertTeacherContactAsync(It.IsAny<TeacherContact>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}
	}
}