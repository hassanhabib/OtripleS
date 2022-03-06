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
        public async Task ShouldAddTeacherContactAsync()
        {
            // given
            TeacherContact randomTeacherContact = CreateRandomTeacherContact();
            TeacherContact inputTeacherContact = randomTeacherContact;
            TeacherContact storageTeacherContact = randomTeacherContact;
            TeacherContact expectedTeacherContact = storageTeacherContact;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact))
                    .ReturnsAsync(storageTeacherContact);

            // when
            TeacherContact actualTeacherContact =
                await this.teacherContactService.AddTeacherContactAsync(inputTeacherContact);

            // then
            actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertTeacherContactAsync(inputTeacherContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }

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