// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

namespace OtripleS.Web.Api.Services.Teachers
{
    public interface ITeacherService
    {
        ValueTask<Teacher> DeleteTeacherByIdAsync(Guid teacherId);
        ValueTask<Teacher> RetrieveTeacherByIdAsync(Guid teacherId);
    }
}
