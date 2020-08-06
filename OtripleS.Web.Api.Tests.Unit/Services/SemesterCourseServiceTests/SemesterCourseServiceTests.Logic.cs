// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.SemesterCourseServiceTests
{
	public partial class SemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldCreateSemesterCourseAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDateTime);
            randomSemesterCourse.UpdatedBy = randomSemesterCourse.CreatedBy;
            randomSemesterCourse.UpdatedDate = randomSemesterCourse.CreatedDate;
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse storageSemesterCourse = randomSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse))
                    .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.CreateSemesterCourseAsync(inputSemesterCourse);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertSemesterCourseAsync(inputSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldDeleteSemesterCourseAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(dateTime);
            Guid inputSemesterCourseId = randomSemesterCourse.Id;
            SemesterCourse inputSemesterCourse = randomSemesterCourse;
            SemesterCourse storageSemesterCourse = inputSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                .ReturnsAsync(inputSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                    broker.DeleteSemesterCourseAsync(inputSemesterCourse))
                .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.DeleteSemesterCourseAsync(inputSemesterCourseId);

            //then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                Times.Once);

            this.storageBrokerMock.Verify(broker =>
                    broker.DeleteSemesterCourseAsync(inputSemesterCourse),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
        
        [Fact]
        public async Task ShouldRetrieveSemesterCourseByIdAsync()
        {
            // given
            Guid randomSemesterCourseId = Guid.NewGuid();
            Guid inputSemesterCourseId = randomSemesterCourseId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            SemesterCourse randomSemesterCourse = CreateRandomSemesterCourse(randomDateTime);
            SemesterCourse storageSemesterCourse = randomSemesterCourse;
            SemesterCourse expectedSemesterCourse = storageSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId))
                    .ReturnsAsync(storageSemesterCourse);

            // when
            SemesterCourse actualSemesterCourse =
                await this.semesterCourseService.RetrieveSemesterCourseByIdAsync(inputSemesterCourseId);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedSemesterCourse);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectSemesterCourseByIdAsync(inputSemesterCourseId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        
        }
    }
}
