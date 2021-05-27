using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentRegistrations.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentRegistrations
{
    public partial class StudentRegistrationServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentRegistrationsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedStudentRegistrationDependencyException =
                new StudentRegistrationDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentRegistrations())
                    .Throws(sqlException);

            // when . // then
            Assert.Throws<StudentRegistrationDependencyException>(() =>
                this.studentRegistrationService.RetrieveAllStudentRegistrations());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is
                    (SameExceptionAs(expectedStudentRegistrationDependencyException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentRegistrations(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
