// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentContacts;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentContacts
{
    public partial class StudentContactServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentStudentContactAsync()
        {
            // given
            StudentContact randomStudentContact = CreateRandomStudentContact();
            StudentContact inputStudentContact = randomStudentContact;
            StudentContact storageStudentContact = randomStudentContact;
            StudentContact expectedStudentContact = storageStudentContact;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentContactAsync(inputStudentContact))
                    .ReturnsAsync(storageStudentContact);

            // when
            StudentContact actualStudentContact =
                await this.studentContactService.AddStudentContactAsync(inputStudentContact);

            // then
            actualStudentContact.Should().BeEquivalentTo(expectedStudentContact);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentContactAsync(inputStudentContact),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
