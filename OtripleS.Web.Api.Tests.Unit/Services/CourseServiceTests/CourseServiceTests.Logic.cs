// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Courses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CourseServiceTests
{
    public partial class CourseServiceTests
    {
        [Fact]
<<<<<<< HEAD
        public async Task ShouldModifyCourseAsync()
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dates: randomDate);

            // when
            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                .Returns(randomDate);
            // then
        }

        [Fact]
=======
        public async Task ShouldDeleteCourseAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Course randomCourse = CreateRandomCourse(dateTime);
            Guid inputCourseId = randomCourse.Id;
            Course inputCourse = randomCourse;
            Course storageCourse = randomCourse;
            Course expectedCourse = randomCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCourseByIdAsync(inputCourseId))
                    .ReturnsAsync(inputCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteCourseAsync(inputCourse))
                    .ReturnsAsync(storageCourse);

            // when
            Course actualCourse =
                await this.courseService.DeleteCourseAsync(inputCourseId);

            // then
            actualCourse.Should().BeEquivalentTo(expectedCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCourseByIdAsync(inputCourseId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteCourseAsync(inputCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
>>>>>>> f475715c79ba835e29d4ab774f2dff6838a08179
        }
    }
}
