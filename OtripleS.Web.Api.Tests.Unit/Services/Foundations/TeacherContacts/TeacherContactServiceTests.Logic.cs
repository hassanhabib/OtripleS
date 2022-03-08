// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.TeacherContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.TeacherContacts
{
    public partial class TeacherContactServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllTeacherContacts()
        {
            //given
            IQueryable<TeacherContact> randomSemesterCourses = CreateRandomTeacherContacts();
            IQueryable<TeacherContact> storageTeacherContacts = randomSemesterCourses;
            IQueryable<TeacherContact> expectedTeacherContacts = storageTeacherContacts;

            this.storageBrokerMock.Setup(broker => broker.SelectAllTeacherContacts())
                .Returns(storageTeacherContacts);

            // when
            IQueryable<TeacherContact> actualTeacherContacts =
                this.teacherContactService.RetrieveAllTeacherContacts();

            actualTeacherContacts.Should().BeEquivalentTo(expectedTeacherContacts);

            this.storageBrokerMock.Verify(broker => broker.SelectAllTeacherContacts(),
                Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }                
    }
}