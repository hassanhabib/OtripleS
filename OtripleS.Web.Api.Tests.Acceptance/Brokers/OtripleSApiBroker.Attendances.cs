using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Attendances;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string AttendanceRelativeUrl = "api/attendances";

        public async ValueTask<Attendance> PostAttendanceAsync(Attendance attendance) =>
            await this.apiFactoryClient.PostContentAsync(AttendanceRelativeUrl, attendance);

        public async ValueTask<Attendance> GetAttendanceByIdAsync(Guid attendanceId) =>
            await this.apiFactoryClient.GetContentAsync<Attendance>($"{AttendanceRelativeUrl}/{attendanceId}");

        public async ValueTask<Attendance> DeleteAttendanceByIdAsync(Guid attendanceId) =>
            await this.apiFactoryClient.DeleteContentAsync<Attendance>($"{AttendanceRelativeUrl}/{attendanceId}");

        public async ValueTask<Attendance> PutAttendanceAsync(Attendance attendance) =>
            await this.apiFactoryClient.PutContentAsync(AttendanceRelativeUrl, attendance);

        public async ValueTask<List<Attendance>> GetAllAttendancesAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Attendance>>($"{AttendanceRelativeUrl}/");
    }
}