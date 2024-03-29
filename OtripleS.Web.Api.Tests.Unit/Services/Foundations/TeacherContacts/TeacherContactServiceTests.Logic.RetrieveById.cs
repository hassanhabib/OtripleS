﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
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
        public async Task ShouldRetrieveTeacherContactById()
        {
            // given
            DateTimeOffset inputDateTime = GetRandomDateTime();
            TeacherContact randomTeacherContact = CreateRandomTeacherContact(inputDateTime);
            TeacherContact storageTeacherContact = randomTeacherContact;
            TeacherContact expectedTeacherContact = storageTeacherContact;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectTeacherContactByIdAsync(randomTeacherContact.TeacherId, randomTeacherContact.ContactId))
                    .ReturnsAsync(randomTeacherContact);

            // when
            TeacherContact actualTeacherContact = await
                this.teacherContactService.RetrieveTeacherContactByIdAsync(
                    randomTeacherContact.TeacherId, randomTeacherContact.ContactId);

            // then
            actualTeacherContact.Should().BeEquivalentTo(expectedTeacherContact);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectTeacherContactByIdAsync(randomTeacherContact.TeacherId, randomTeacherContact.ContactId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
