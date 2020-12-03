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

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Calenders
{
    public partial class CalendersApiTest
    {

        [Fact]
        public async Task ShouldPostCalendarAsync()
        {
            // given
           Calendar randomCalendar = CreateRandomCalender();
           Calendar inputCalendar = randomCalendar;
           Calendar expectedCalendar = inputCalendar;

            // when 
            await this.otripleSApiBroker.PostCalendarAsync(inputCalendar);

           Calendar actualCalendar =
                await this.otripleSApiBroker.GetCalenderByIdAsync(inputCalendar.Id);

            // then
            actualCalendar.Should().BeEquivalentTo(expectedCalendar);
            await this.otripleSApiBroker.DeleteCalenderByIdAsync(actualCalendar.Id);
        }
      
        [Fact]
        public async Task ShouldPutCalendarAsync()
        {
            // given
            Calendar randomCalendar = await PostRandomCalenderAsync();
            Calendar modifiedCalendar = UpdateCalendarRandom(randomCalendar);

            // when
            await this.otripleSApiBroker.PutCalendarAsync(modifiedCalendar);

            Calendar actualCalendar =
                await this.otripleSApiBroker.GetCalenderByIdAsync(randomCalendar.Id);

            // then
            actualCalendar.Should().BeEquivalentTo(modifiedCalendar);
            await this.otripleSApiBroker.DeleteCalenderByIdAsync(actualCalendar.Id);
        }


        [Fact]
        public async Task ShouldGetAllCalendersAsync()
        {
            //given
            var randomCalenders = new List<Calendar>();
            
            for (var i =0; i <= GetRandomNumber(); i++)
            {
                randomCalenders.Add(await PostRandomCalenderAsync());
            }

            List<Calendar> inputedCalenders = randomCalenders;
            List<Calendar> expectedCalenders = inputedCalenders.ToList();

            //when 
            List<Calendar> actualCalenders = await this.otripleSApiBroker.GetAllCalendersAsync();

            //then
            foreach (var expectcalendar in expectedCalenders)
            {
                Calendar actualCalender = actualCalenders.Single(calendar => calendar.Id == expectcalendar.Id);

                actualCalender.Should().BeEquivalentTo(expectcalendar);
                await this.otripleSApiBroker.DeleteCalenderByIdAsync(actualCalender.Id);
            }
        }

        [Fact]
        public async Task ShouldDeleteCalenderAsync()
        {
            //given
            Calendar randomCalender = await PostRandomCalenderAsync();
            Calendar inputCalender = randomCalender;
            Calendar expectedCalender = inputCalender;

            //when
            Calendar deletedCalender = 
                await this.otripleSApiBroker.DeleteCalenderByIdAsync(inputCalender.Id);

            ValueTask<Calendar> getCalenderByIdTask = 
                this.otripleSApiBroker.DeleteCalenderByIdAsync(inputCalender.Id);

            // then
            deletedCalender.Should().BeEquivalentTo(expectedCalender);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalenderByIdTask.AsTask());
        }
    }
}
