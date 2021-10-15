// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Teachers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public interface ITeacherService
    {
        ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId);
        ValueTask<Teacher> RemoveTeacherByIdAsync(Guid teacherId);
        IQueryable<Teacher> RetrieveAllTeachers();
        ValueTask<Teacher> CreateTeacherAsync(Teacher teacher);
        ValueTask<Teacher> ModifyTeacherAsync(Teacher teacher);
    }
}
