// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Classrooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
    public partial class OtripleSApiBroker
    {
        private const string ClassrommRelativeUrl = "api/Classrooms";

        public async ValueTask<Classroom> PostClassroomAsync(Classroom classroom) =>
            await this.apiFactoryClient.PostContentAsync(ClassrommRelativeUrl, classroom);

        public async ValueTask<Classroom> GetClassroomByIdAsync(Guid classroomId) =>
            await this.apiFactoryClient.GetContentAsync<Classroom>($"{ClassrommRelativeUrl}/{classroomId}");

        public async ValueTask<Classroom> DeleteClassroomByIdAsync(Guid classroomId) =>
            await this.apiFactoryClient.DeleteContentAsync<Classroom>($"{ClassrommRelativeUrl}/{classroomId}");

        public async ValueTask<Classroom> PutClassroomAsync(Classroom classroom) =>
            await this.apiFactoryClient.PutContentAsync(ClassrommRelativeUrl, classroom);

        public async ValueTask<List<Classroom>> GetAllClassroomAsync() =>
           await this.apiFactoryClient.GetContentAsync<List<Classroom>>($"{ClassrommRelativeUrl}/");
    }
}
