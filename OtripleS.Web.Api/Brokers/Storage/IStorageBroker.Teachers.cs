// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Teachers;

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
