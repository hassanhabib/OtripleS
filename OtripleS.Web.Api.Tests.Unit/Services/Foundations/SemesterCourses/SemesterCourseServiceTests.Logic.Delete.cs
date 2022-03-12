﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.SemesterCourses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.SemesterCourses
{
    public partial class SemesterCourseServiceTests
    {
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
                await this.semesterCourseService.RemoveSemesterCourseByIdAsync(inputSemesterCourseId);

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

    }
}
