// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnDeleteWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomClassroomId = Guid.NewGuid();
            Guid inputClassroomId = randomClassroomId;
            SqlException sqlException = GetSqlException();

            var expectedClassroomDependencyException =
                new ClassroomDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Classroom> deleteClassroomTask =
                this.classroomService.DeleteClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                deleteClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedClassroomDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
