// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attendances;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Attendances
{
    public partial class AttendancesApiTests
    {
        [Fact]
        public async Task ShouldGetAllAttendancesAsync()
        {
            // given
            IEnumerable<Attendance> randomAttendances = GetRandomAttendances();
            IEnumerable<Attendance> inputAttendances = randomAttendances;

            foreach (Attendance attendance in inputAttendances)
            {
                await this.otripleSApiBroker.PostAttendanceAsync(attendance);
            }

            List<Attendance> expectedAttendances = inputAttendances.ToList();

            // when
            List<Attendance> actualAttendances = await this.otripleSApiBroker.GetAllAttendancesAsync();

            // then
            foreach (Attendance expectedAttendance in expectedAttendances)
            {
                Attendance actualAttendance =
                    actualAttendances.Single(attendance => attendance.Id == expectedAttendance.Id);

                actualAttendance.Should().BeEquivalentTo(expectedAttendance);
                await this.otripleSApiBroker.DeleteAttendanceByIdAsync(actualAttendance.Id);
            }
        }

        [Fact]
        public async Task ShouldModifyAttendanceAsync()
        {
            // given
            Attendance randomAttendance = CreateRandomAttendance();
            await this.otripleSApiBroker.PostAttendanceAsync(randomAttendance);
            Attendance modifiedAttendance = UpdateAttendanceRandom(randomAttendance);

            // when
            await this.otripleSApiBroker.PutAttendanceAsync(modifiedAttendance);

            Attendance actualAttendance =
                await this.otripleSApiBroker.GetAttendanceByIdAsync(randomAttendance.Id);

            // then
            actualAttendance.Should().BeEquivalentTo(modifiedAttendance);
            await this.otripleSApiBroker.DeleteAttendanceByIdAsync(actualAttendance.Id);
        }

        [Fact]
        public async Task ShouldPostAttendanceAsync()
        {
            // given
            Attendance randomAttendance = CreateRandomAttendance();
            Attendance inputAttendance = randomAttendance;
            Attendance expectedAttendance = inputAttendance;

            // when 
            await this.otripleSApiBroker.PostAttendanceAsync(inputAttendance);

            Attendance actualAttendance =
                await this.otripleSApiBroker.GetAttendanceByIdAsync(inputAttendance.Id);

            // then
            actualAttendance.Should().BeEquivalentTo(expectedAttendance);
            await this.otripleSApiBroker.DeleteAttendanceByIdAsync(actualAttendance.Id);
        }
    }
}