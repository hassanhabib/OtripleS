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
