using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string TeachersRelativeUrl = "api/teachers";

        public async ValueTask<Teacher> PostTeacherAsync(Teacher teacher) =>
            await this.apiFactoryClient.PostContentAsync(TeachersRelativeUrl, teacher);

        public async ValueTask<Teacher> GetTeacherByIdAsync(Guid teacherId) =>
            await this.apiFactoryClient.GetContentAsync<Teacher>($"{TeachersRelativeUrl}/{teacherId}");

        public async ValueTask<Teacher> DeleteTeacherByIdAsync(Guid teacherId) =>
            await this.apiFactoryClient.DeleteContentAsync<Teacher>($"{TeachersRelativeUrl}/{teacherId}");

        public async ValueTask<Teacher> PutTeacherAsync(Teacher teacher) =>
            await this.apiFactoryClient.PutContentAsync(TeachersRelativeUrl, teacher);

        public async ValueTask<List<Teacher>> GetAllTeachers() =>
            await this.apiFactoryClient.GetContentAsync<List<Teacher>>($"{TeachersRelativeUrl}/");

    }
}
