using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomStudentGuardianId = Guid.NewGuid();
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentGuardianId = randomStudentGuardianId;
            Guid inputStudentId = randomStudentId;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentGuardianDependencyException =
                new StudentGuardianDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentGuardianByIdAsync(inputStudentGuardianId, inputStudentId))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentGuardian> deleteStudentGuardianTask =
                this.studentGuardianService.DeleteStudentGuardianAsync(inputStudentGuardianId, inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentGuardianDependencyException>(() => deleteStudentGuardianTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentGuardianDependencyException))),
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
