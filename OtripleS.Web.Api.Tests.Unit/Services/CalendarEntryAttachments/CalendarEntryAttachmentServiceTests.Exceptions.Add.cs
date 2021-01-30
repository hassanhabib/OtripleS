//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            var sqlException = GetSqlException();
            
            var expectedCalendarEntryAttachmentDependencyException = 
                new CalendarEntryAttachmentDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() => 
                addCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnAddWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryAttachmentDependencyException =
                new CalendarEntryAttachmentDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentDependencyException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnAddWhenExceptionOccursAndLogItAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            var exception = new Exception();
            var expectedCalendarEntryAttachmentServiceException = new CalendarEntryAttachmentServiceException(exception);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment))
                    .ThrowsAsync(exception);

            // when
            ValueTask<CalendarEntryAttachment> addCalendarEntryAttachmentTask =
                 this.calendarEntryAttachmentService.AddCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            // then
            await Assert.ThrowsAsync<CalendarEntryAttachmentServiceException>(() =>
                addCalendarEntryAttachmentTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryAttachmentServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAttachmentAsync(inputCalendarEntryAttachment),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
