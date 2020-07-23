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
        public async Task ShouldThrowDependencyExceptionOnRetrieveByIdWhenSqlExceptionOccursAndLogItAsync()
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
            ValueTask<Classroom> retrieveClassroomTask =
                this.classroomService.RetrieveClassroomById(inputClassroomId);
            
            // then
            await Assert.ThrowsAsync<ClassroomDependencyException>(() =>
                retrieveClassroomTask.AsTask());
            
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