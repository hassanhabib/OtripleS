// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
