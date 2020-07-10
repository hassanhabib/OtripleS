// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Services.Teachers
{
    public interface ITeacherService
    {
        ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId);
        ValueTask<Teacher> DeleteTeacherByIdAsync(Guid teacherId);
        IQueryable<Teacher> RetrieveAllTeachers();
        ValueTask<Teacher> CreateTeacherAsync(Teacher teacher);
        ValueTask<Teacher> ModifyTeacherAsync(Teacher teacher);
    }
}
