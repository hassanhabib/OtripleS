// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentSemesterCourses
{
    public partial class StudentSemesterCourseServiceTests
    {       

        [Fact]
        public void ShouldRetrieveAllStudentSemesterCourses()
        {
            //given
            IQueryable<StudentSemesterCourse> randomSemesterCourses =
                CreateRandomStudentSemesterCourses();

            IQueryable<StudentSemesterCourse> storageStudentSemesterCourses = randomSemesterCourses;
            IQueryable<StudentSemesterCourse> expectedStudentSemesterCourses = storageStudentSemesterCourses;

            this.storageBrokerMock.Setup(broker => broker.SelectAllStudentSemesterCourses())
                .Returns(storageStudentSemesterCourses);

            // when
            IQueryable<StudentSemesterCourse> actualStudentSemesterCourses =
                this.studentSemesterCourseService.RetrieveAllStudentSemesterCourses();

            actualStudentSemesterCourses.Should().BeEquivalentTo(expectedStudentSemesterCourses);

            this.storageBrokerMock.Verify(broker => broker.SelectAllStudentSemesterCourses(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }        
    }
}
