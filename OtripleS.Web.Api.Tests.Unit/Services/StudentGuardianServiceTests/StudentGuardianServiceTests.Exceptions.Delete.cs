using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.StudentGuardians;
using OtripleS.Web.Api.Models.StudentGuardians.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentGuardianServiceTests
{
   public partial class StudentGuardianServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            SqlException sqlException = GetSqlException();

            var expectedStudentGuardianDependencyException
                = new StudentGuardianDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                 broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() =>
                deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }
    }
}
