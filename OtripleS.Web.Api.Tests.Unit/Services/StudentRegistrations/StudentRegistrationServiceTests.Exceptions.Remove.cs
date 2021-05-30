// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId; 
            Guid randomRegistrationId = Guid.NewGuid();
            Guid inputRegistrationId = randomRegistrationId;            
            SqlException sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentRegistration> deleteStudentRegistrationTask =
                this.studentRegistrationService.RemoveStudentRegistrationByIdsAsync(
                    inputRegistrationId, 
                    inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentRegistrationDependencyException>(() =>
                deleteStudentRegistrationTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentRegistrationByIdAsync(inputRegistrationId, inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentRegistrationDependencyException))),
                    Times.Once);
            
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }


    }
}
