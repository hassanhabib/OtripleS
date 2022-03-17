// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Services.Foundations.Teachers
{
    public interface ITeacherService
    {
        ValueTask<Teacher> CreateTeacherAsync(Teacher teacher);
        IQueryable<Teacher> RetrieveAllTeachers();
        ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId);
        ValueTask<Teacher> ModifyTeacherAsync(Teacher teacher);
        ValueTask<Teacher> RemoveTeacherByIdAsync(Guid teacherId);
    }
}
