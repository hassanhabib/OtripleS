// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Classrooms;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Classrooms
{
    public interface IClassroomService
    {
        ValueTask<Classroom> CreateClassroomAsync(Classroom classroom);
        ValueTask<Classroom> GetClassroomById(Guid classroomId);
        ValueTask<Classroom> DeleteClassroomAsync(Guid classroomId);
        ValueTask<Classroom> ModifyClassroomAsync(Classroom classroom);
        IQueryable<Classroom> RetrieveAllClassrooms();
    }
}
