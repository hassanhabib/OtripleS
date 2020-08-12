using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldCreateStudentSemesterCourseAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            StudentSemesterCourse randomSemesterCourse = CreateRandomStudentSemesterCourse(randomDateTime);
            randomSemesterCourse.UpdatedBy = randomSemesterCourse.CreatedBy;
            randomSemesterCourse.UpdatedDate = randomSemesterCourse.CreatedDate;
            StudentSemesterCourse inputStudentSemesterCourse = randomSemesterCourse;
            StudentSemesterCourse storageStudentSemesterCourse = randomSemesterCourse;
            StudentSemesterCourse expectedStudentSemesterCourse = storageStudentSemesterCourse;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ReturnsAsync(storageStudentSemesterCourse);

            // when
            StudentSemesterCourse actualSemesterCourse =
                await this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            actualSemesterCourse.Should().BeEquivalentTo(expectedStudentSemesterCourse);

            // This is called within validation code
            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
