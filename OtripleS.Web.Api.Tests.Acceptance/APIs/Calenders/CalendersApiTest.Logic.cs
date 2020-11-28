// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Calendars;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Calenders
{
    public partial class CalendersApiTest
    {
        [Fact]
        public async Task ShouldGetAllCalendersAsync()
        {
            //given
            IEnumerable<Calendar> randomCalenders = GetRandomCalenders();
            IEnumerable<Calendar> inputedCalenders = randomCalenders;

            foreach(var calender in inputedCalenders)
            {
              await  this.otripleSApiBroker.PostCalendarAsync(calender);
            }

            List<Calendar> expectedCalenders = inputedCalenders.ToList();

            //when 
            List<Calendar> actualCalenders =   await this.otripleSApiBroker.GetAllCalendersAsync();

            //then
            foreach (var expectcalender in expectedCalenders)
            {
                Calendar actualCalender = actualCalenders.Single(calender => calender.Id == expectcalender.Id);

                actualCalender.Should().BeEquivalentTo(expectcalender);
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
            Calendar deletedCalender = await this.otripleSApiBroker.DeleteCalenderByIdAsync(inputCalender.Id);

            ValueTask<Calendar> getCalenderByIdTask =  this.otripleSApiBroker.DeleteCalenderByIdAsync(inputCalender.Id);

            // then
            deletedCalender.Should().BeEquivalentTo(expectedCalender);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getCalenderByIdTask.AsTask());
        }
    }
}
