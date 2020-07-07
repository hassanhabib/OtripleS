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
        public ValueTask<Teacher> InsertTeacherAsync(Teacher teacher);
        public IQueryable<Teacher> SelectAllTeachers();
        public ValueTask<Teacher> SelectTeacherByIdAsync(Guid teacherId);
        public ValueTask<Teacher> UpdateTeacherAsync(Teacher teacher);
        public ValueTask<Teacher> DeleteTeacherAsync(Teacher teacher);
    }
}
