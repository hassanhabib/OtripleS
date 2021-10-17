// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.TeacherContacts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<TeacherContact> InsertTeacherContactAsync(TeacherContact teacherContact);
        IQueryable<TeacherContact> SelectAllTeacherContacts();
        ValueTask<TeacherContact> SelectTeacherContactByIdAsync(Guid teacherId, Guid contactId);
        ValueTask<TeacherContact> UpdateTeacherContactAsync(TeacherContact teacherContact);
        ValueTask<TeacherContact> DeleteTeacherContactAsync(TeacherContact teacherContact);
    }
}