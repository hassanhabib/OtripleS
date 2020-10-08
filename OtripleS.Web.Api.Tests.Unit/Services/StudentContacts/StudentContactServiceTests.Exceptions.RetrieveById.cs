// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using OtripleS.Web.Api.Models.StudentContacts.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomContactId = Guid.NewGuid();
            Guid inputContactId = randomContactId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentContactDependencyException
                = new StudentContactDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentContact> deleteStudentContactTask =
                this.studentContactService.RetrieveStudentContactByIdAsync
                (inputStudentId, inputContactId);

            // then
            await Assert.ThrowsAsync<StudentContactDependencyException>(() =>
                deleteStudentContactTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentContactDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentContactByIdAsync(inputStudentId, inputContactId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
