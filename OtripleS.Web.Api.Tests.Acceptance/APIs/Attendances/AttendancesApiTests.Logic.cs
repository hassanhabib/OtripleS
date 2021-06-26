// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attendances;
using RESTFulSense.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attendances
{
    public partial class AttendancesApiTests
    {
        [Fact]
        public async Task ShouldGetAllAttendancesAsync()
        {
            // given
            var randomAttendances = new List<Attendance>();

            for (int i = 0; i <= GetRandomNumber(); i++)
            {
                randomAttendances.Add(await PostRandomAttendanceAsync());
            }

            List<Attendance> inputAttendances = randomAttendances;
            List<Attendance> expectedAttendances = inputAttendances.ToList();

            // when
            List<Attendance> actualAttendances = await this.otripleSApiBroker.GetAllAttendancesAsync();

            // then
            foreach (Attendance expectedAttendance in expectedAttendances)
            {
                Attendance actualAttendance =
                    actualAttendances.Single(attendance => attendance.Id == expectedAttendance.Id);

                actualAttendance.Should().BeEquivalentTo(expectedAttendance);
                await DeleteAttendanceByIdAsync(actualAttendance);
            }
        }

        [Fact]
        public async Task ShouldModifyAttendanceAsync()
        {
            // given
            Attendance randomAttendance = await PostRandomAttendanceAsync();
            Attendance modifiedAttendance = UpdateAttendanceRandom(randomAttendance);

            // when
            await this.otripleSApiBroker.PutAttendanceAsync(modifiedAttendance);

            Attendance actualAttendance =
                await this.otripleSApiBroker.GetAttendanceByIdAsync(randomAttendance.Id);

            // then
            actualAttendance.Should().BeEquivalentTo(modifiedAttendance);
            await DeleteAttendanceByIdAsync(actualAttendance);
        }

        [Fact]
        public async Task ShouldPostAttendanceAsync()
        {
            // given
            Attendance randomAttendance = await CreateRandomAttendanceAsync();
            Attendance inputAttendance = randomAttendance;
            Attendance expectedAttendance = inputAttendance;

            // when 
            await this.otripleSApiBroker.PostAttendanceAsync(inputAttendance);

            Attendance actualAttendance =
                await this.otripleSApiBroker.GetAttendanceByIdAsync(inputAttendance.Id);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);
            await DeleteAttendanceByIdAsync(actualAttendance);
        }

        [Fact]
        public async Task ShouldDeleteAttendanceAsync()
        {
            // given
            Attendance randomAttendance = await PostRandomAttendanceAsync();
            Attendance inputAttendance = randomAttendance;
            Attendance expectedAttendance = inputAttendance;

            // when 
            Attendance deletedAttendance =
                await DeleteAttendanceByIdAsync(inputAttendance);

            ValueTask<Attendance> getAttendanceByIdTask =
                this.otripleSApiBroker.GetAttendanceByIdAsync(inputAttendance.Id);

            // then
            deletedAttendance.Should().BeEquivalentTo(expectedAttendance);

            await Assert.ThrowsAsync<HttpResponseNotFoundException>(() =>
               getAttendanceByIdTask.AsTask());
        }
    }
}