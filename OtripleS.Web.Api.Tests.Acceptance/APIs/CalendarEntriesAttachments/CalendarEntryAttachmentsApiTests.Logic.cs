// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.CalendarEntriesAttachments;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentsApiTests
    {
        [Fact]
        public async Task ShouldPostCalendarEntryAttachmentAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = await CreateRandomCalendarEntryAttachment();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = inputCalendarEntryAttachment;

            // when             
            CalendarEntryAttachment actualCalendarEntryAttachment =
                await this.otripleSApiBroker.PostCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            CalendarEntryAttachment retrievedCalendarEntryAttachment =
                await this.otripleSApiBroker.GetCalendarEntryAttachmentByIdsAsync(
                    inputCalendarEntryAttachment.CalendarEntryId,
                    inputCalendarEntryAttachment.AttachmentId);

            // then
            actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);
            retrievedCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);
            await DeleteCalendarEntryAttachmentAsync(actualCalendarEntryAttachment);
        }

        [Fact]
        public async Task ShouldGetAllCalendarEntryAttachmentsAsync()
        {
            // given
            var randomCalendarEntryAttachments = new List<CalendarEntryAttachment>();

            for (var i = 0; i <= GetRandomNumber(); i++)
            {
                CalendarEntryAttachment randomCalendarEntryAttachment = await PostCalendarEntryAttachmentAsync();
                randomCalendarEntryAttachments.Add(randomCalendarEntryAttachment);
            }

            List<CalendarEntryAttachment> inputCalendarEntryAttachments = randomCalendarEntryAttachments;
            List<CalendarEntryAttachment> expectedCalendarEntryAttachments = inputCalendarEntryAttachments;

            // when
            List<CalendarEntryAttachment> actualCalendarEntryAttachments =
                await this.otripleSApiBroker.GetAllCalendarEntryAttachmentsAsync();

            // then
            foreach (CalendarEntryAttachment expectedCalendarEntryAttachment in expectedCalendarEntryAttachments)
            {
                CalendarEntryAttachment actualCalendarEntryAttachment =
                    actualCalendarEntryAttachments.Single(studentAttachment =>
                        studentAttachment.CalendarEntryId == expectedCalendarEntryAttachment.CalendarEntryId);

                actualCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

                await DeleteCalendarEntryAttachmentAsync(actualCalendarEntryAttachment);
            }
        }

        [Fact]
        public async Task ShouldDeleteCalendarEntryAttachmentAsync()
        {
            // given
            CalendarEntryAttachment randomCalendarEntryAttachment = await PostCalendarEntryAttachmentAsync();
            CalendarEntryAttachment inputCalendarEntryAttachment = randomCalendarEntryAttachment;
            CalendarEntryAttachment expectedCalendarEntryAttachment = inputCalendarEntryAttachment;

            // when 
            CalendarEntryAttachment deletedCalendarEntryAttachment =
                await DeleteCalendarEntryAttachmentAsync(inputCalendarEntryAttachment);

            ValueTask<CalendarEntryAttachment> getCalendarEntryAttachmentByIdTask =
                this.otripleSApiBroker.GetCalendarEntryAttachmentByIdsAsync(
                    inputCalendarEntryAttachment.CalendarEntryId,
                    inputCalendarEntryAttachment.AttachmentId);

            // then
            deletedCalendarEntryAttachment.Should().BeEquivalentTo(expectedCalendarEntryAttachment);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalendarEntryAttachmentByIdTask.AsTask());
        }
    }
}
