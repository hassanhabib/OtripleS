using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentAttachmentsWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedStudentAttachmentDependencyException =
                new StudentAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<StudentAttachmentDependencyException>(() =>
                this.studentAttachmentService.RetrieveAllStudentAttachments());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllStudentAttachmentsWhenDbExceptionOccursAndLogIt()
        {
            // given
            var databaseUpdateException = new DbUpdateException();

            var expectedAttachmentDependencyException =
                new StudentAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Throws(databaseUpdateException);

            // when . then
            Assert.Throws<StudentAttachmentDependencyException>(() =>
                this.studentAttachmentService.RetrieveAllStudentAttachments());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
