// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using OtripleS.Web.Api.Models.SemesterCourses.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = inputSemesterCourse.CreatedBy;
            inputSemesterCourse.UpdatedDate = inputSemesterCourse.CreatedDate;
            var sqlException = GetSqlException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = inputSemesterCourse.CreatedBy;
            inputSemesterCourse.UpdatedDate = inputSemesterCourse.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedSemesterCourseDependencyException =
                new SemesterCourseDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseDependencyException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            inputSemesterCourse.UpdatedBy = inputSemesterCourse.CreatedBy;
            inputSemesterCourse.UpdatedDate = inputSemesterCourse.CreatedDate;
            var serviceException = new Exception();

            var expectedSemesterCourseServiceException =
                new SemesterCourseServiceException(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse))
                    .ThrowsAsync(serviceException);

            // when
            ValueTask<SemesterCourse> createSemesterCourseTask =
                this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            await Assert.ThrowsAsync<SemesterCourseServiceException>(() =>
                createSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedSemesterCourseServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
