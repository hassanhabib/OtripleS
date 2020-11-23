// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Models.Classrooms;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ClassroomsRelativeUrl = "api/classrooms";

        public async ValueTask<Classroom> PostClassroomAsync(Classroom classroom) =>
            await this.apiFactoryClient.PostContentAsync(ClassroomsRelativeUrl, classroom);

        public async ValueTask<Classroom> GetClassroomByIdAsync(Guid classroomId) =>
            await this.apiFactoryClient.GetContentAsync<Classroom>($"{ClassroomsRelativeUrl}/{classroomId}");

        public async ValueTask<Classroom> DeleteClassroomByIdAsync(Guid classroomId) =>
            await this.apiFactoryClient.DeleteContentAsync<Classroom>($"{ClassroomsRelativeUrl}/{classroomId}");

        public async ValueTask<Classroom> PutClassroomAsync(Classroom classroom) =>
            await this.apiFactoryClient.PutContentAsync(ClassroomsRelativeUrl, classroom);

        public async ValueTask<List<Classroom>> GetAllClassroomsAsync() =>
            await this.apiFactoryClient.GetContentAsync<List<Classroom>>($"{ClassroomsRelativeUrl}/");
    }
}
