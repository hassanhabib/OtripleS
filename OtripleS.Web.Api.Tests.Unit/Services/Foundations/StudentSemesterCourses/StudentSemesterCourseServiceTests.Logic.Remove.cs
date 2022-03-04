using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldDeleteStudentSemesterCourseAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            Guid inputSemesterCourseId = randomStudentSemesterCourse.SemesterCourseId;
            Guid inputStudentId = randomStudentSemesterCourse.StudentId;
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            StudentSemesterCourse storageStudentSemesterCourse = inputStudentSemesterCourse;
            StudentSemesterCourse expectedStudentSemesterCourse = storageStudentSemesterCourse;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId))
                    .ReturnsAsync(inputStudentSemesterCourse);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ReturnsAsync(storageStudentSemesterCourse);

            // when
            StudentSemesterCourse actualStudentSemesterCourse =
                await this.studentSemesterCourseService.RemoveStudentSemesterCourseByIdsAsync(inputSemesterCourseId, inputStudentId);

            actualStudentSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentSemesterCourseByIdAsync(inputSemesterCourseId, inputStudentId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
