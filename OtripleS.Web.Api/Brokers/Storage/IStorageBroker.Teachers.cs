// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Teachers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        ValueTask<Teacher> InsertTeacherAsync(Teacher teacher);
        IQueryable<Teacher> SelectAllTeachers();
        ValueTask<Teacher> SelectTeacherByIdAsync(Guid teacherId);
        ValueTask<Teacher> UpdateTeacherAsync(Teacher teacher);
        ValueTask<Teacher> DeleteTeacherAsync(Teacher teacher);
    }
}
