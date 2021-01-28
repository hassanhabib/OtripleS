using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using OtripleS.Web.Api.Models.CalendarEntryAttachments.Exceptions;
using OtripleS.Web.Api.Models.CalendarEntryAttachments;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntryAttachments
{
    public partial class CalendarEntryAttachmentServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRemoveWhenCalendarEntryIdIsInvalidAndLogItAsync()
        {
            // given
            var randomAttachmentId = Guid.NewGuid();
            Guid randomCalendarEntryId = default;
            var inputAttachmentId = randomAttachmentId;
            var inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarEntryAttachmentInputException = new InvalidCalendarEntryAttachmentException(
                parameterName: nameof(CalendarEntryAttachment.CalendarEntryId),
                parameterValue: inputCalendarEntryId);

            var expectedCalendarEntryAttachmentValidationException =
                new CalendarEntryAttachmentValidationException(invalidCalendarEntryAttachmentInputException);

            // when
            

            // then
        }
    }
}
