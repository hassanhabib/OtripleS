// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Moq;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public void ShouldThrowDependencyExceptionOnRetrieveAllWhenSqlExceptionOccursAndLogIt()
        {
            // given
            var sqlException = GetSqlException();

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllClassrooms())
                    .Throws(sqlException);

            // when . then
            Assert.Throws<ClassroomDependencyException>(() =>
                this.classroomService.RetrieveAllClassrooms());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllClassrooms(),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
