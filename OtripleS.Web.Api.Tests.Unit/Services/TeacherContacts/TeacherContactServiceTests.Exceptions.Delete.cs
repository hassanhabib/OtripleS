// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using OtripleS.Web.Api.Models.TeacherContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.TeacherContacts
{
	public partial class TeacherContactServiceTests
	{
		[Fact]
		public async Task ShouldThrowDependencyExceptionOnRemoveWhenSqlExceptionOccursAndLogItAsync()
		{
			// given
			var randomContactId = Guid.NewGuid();
			var randomTeacherId = Guid.NewGuid();
			Guid someContactId = randomContactId;			
			Guid someTeacherId = randomTeacherId;
			SqlException sqlException = GetSqlException();

			var expectedTeacherContactDependencyException
				= new TeacherContactDependencyException(sqlException);

			this.storageBrokerMock.Setup(broker =>
				 broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId))
					.ThrowsAsync(sqlException);

			// when
			ValueTask<TeacherContact> removeTeacherContactTask =
				this.teacherContactService.RemoveTeacherContactByIdAsync(
					someTeacherId, 
					someContactId);

			// then
			await Assert.ThrowsAsync<TeacherContactDependencyException>(() =>
				removeTeacherContactTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogCritical(It.Is(SameExceptionAs(expectedTeacherContactDependencyException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectTeacherContactByIdAsync(someTeacherId, someContactId),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.DeleteTeacherContactAsync(It.IsAny<TeacherContact>()),
					Times.Never);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}		
	}
}