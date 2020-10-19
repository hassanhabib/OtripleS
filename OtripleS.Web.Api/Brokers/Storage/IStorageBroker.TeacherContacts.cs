// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.TeacherContacts;

namespace OtripleS.Web.Api.Brokers.Storage
{
    public partial interface IStorageBroker
    {
        public ValueTask<TeacherContact> InsertTeacherContactAsync(TeacherContact teacherContact);
        public IQueryable<TeacherContact> SelectAllTeacherContacts();
        public ValueTask<TeacherContact> SelectTeacherContactByIdAsync(Guid teacherId, Guid contactId);
        public ValueTask<TeacherContact> UpdateTeacherContactAsync(TeacherContact teacherContact);
        public ValueTask<TeacherContact> DeleteTeacherContactAsync(TeacherContact teacherContact);
    }
}
