// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Calendars;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Calendars
{
    public partial class CalendarsApiTest
    {

        [Fact]
        public async Task ShouldPostCalendarAsync()
        {
            // given
           Calendar randomCalendar = CreateRandomCalendar();
           Calendar inputCalendar = randomCalendar;
           Calendar expectedCalendar = inputCalendar;

            // when 
            await this.otripleSApiBroker.PostCalendarAsync(inputCalendar);

           Calendar actualCalendar =
                await this.otripleSApiBroker.GetCalendarByIdAsync(inputCalendar.Id);

            // then
            actualCalendar.Should().BeEquivalentTo(expectedCalendar);
            await this.otripleSApiBroker.DeleteCalendarByIdAsync(actualCalendar.Id);
        }
      
        [Fact]
        public async Task ShouldPutCalendarAsync()
        {
            // given
            Calendar randomCalendar = await PostRandomCalendarAsync();
            Calendar modifiedCalendar = UpdateCalendarRandom(randomCalendar);

            // when
            await this.otripleSApiBroker.PutCalendarAsync(modifiedCalendar);

            Calendar actualCalendar =
                await this.otripleSApiBroker.GetCalendarByIdAsync(randomCalendar.Id);

            // then
            actualCalendar.Should().BeEquivalentTo(modifiedCalendar);
            await this.otripleSApiBroker.DeleteCalendarByIdAsync(actualCalendar.Id);
        }


        [Fact]
        public async Task ShouldGetAllCalendarsAsync()
        {
            //given
            var randomCalendars = new List<Calendar>();
            
            for (var i =0; i <= GetRandomNumber(); i++)
            {
                randomCalendars.Add(await PostRandomCalendarAsync());
            }

            List<Calendar> inputedCalendars = randomCalendars;
            List<Calendar> expectedCalendars = inputedCalendars.ToList();

            //when 
            List<Calendar> actualCalendars = await this.otripleSApiBroker.GetAllCalendarsAsync();

            //then
            foreach (var expectcalendar in expectedCalendars)
            {
                Calendar actualCalendar = actualCalendars.Single(calendar => calendar.Id == expectcalendar.Id);

                actualCalendar.Should().BeEquivalentTo(expectcalendar);
                await this.otripleSApiBroker.DeleteCalendarByIdAsync(actualCalendar.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteCalendarAsync()
        {
            //given
            Calendar randomCalendar = await PostRandomCalendarAsync();
            Calendar inputCalendar = randomCalendar;
            Calendar expectedCalendar = inputCalendar;

            //when
            Calendar deletedCalendar = 
                await this.otripleSApiBroker.DeleteCalendarByIdAsync(inputCalendar.Id);

            ValueTask<Calendar> getCalendarByIdTask = 
                this.otripleSApiBroker.DeleteCalendarByIdAsync(inputCalendar.Id);

            // then
            deletedCalendar.Should().BeEquivalentTo(expectedCalendar);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalendarByIdTask.AsTask());
        }
    }
}
