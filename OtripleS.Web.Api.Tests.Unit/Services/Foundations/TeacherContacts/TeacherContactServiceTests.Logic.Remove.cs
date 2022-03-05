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
        public async Task ShouldRemoveTeacherContactAsync()
        {
            // given
            var randomTeacherId = Guid.NewGuid();
            var randomContactId = Guid.NewGuid();
            Guid inputTeacherId = randomTeacherId;
            Guid inputContactId = randomContactId;
            DateTimeOffset inputDateTime = GetRandomDateTime();
            TeacherContact randomTeacherContact = CreateRandomTeacherContact(inputDateTime);
            randomTeacherContact.TeacherId = inputTeacherId;
            randomTeacherContact.ContactId = inputContactId;
            TeacherContact storageTeacherContact = randomTeacherContact;
            TeacherContact expectedTeacherContact = storageTeacherContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId))
                    .ReturnsAsync(storageTeacherContact);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteTeacherContactAsync(storageTeacherContact))
                    .ReturnsAsync(expectedTeacherContact);

            // when
            TeacherContact actualTeacherContact =
                await this.teacherContactService.RemoveTeacherContactByIdAsync(inputTeacherId, inputContactId);

            // then
            actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(inputTeacherId, inputContactId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteTeacherContactAsync(storageTeacherContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
