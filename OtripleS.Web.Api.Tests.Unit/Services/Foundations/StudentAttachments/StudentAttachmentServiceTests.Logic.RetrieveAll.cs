﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentAttachments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentAttachments
{
    public partial class StudentAttachmentServiceTests
    {
        [Fact]
        public void ShouldRetrieveAllStudentAttachments()
        {
            // given
            IQueryable<StudentAttachment> randomStudentAttachments = CreateRandomStudentAttachments();
            IQueryable<StudentAttachment> storageStudentAttachments = randomStudentAttachments;
            IQueryable<StudentAttachment> expectedStudentAttachments = storageStudentAttachments;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllStudentAttachments())
                    .Returns(storageStudentAttachments);

            // when
            IQueryable<StudentAttachment> actualStudentAttachments =
                this.studentAttachmentService.RetrieveAllStudentAttachments();

            // then
            actualStudentAttachments.Should().BeEquivalentTo(expectedStudentAttachments);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllStudentAttachments(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
