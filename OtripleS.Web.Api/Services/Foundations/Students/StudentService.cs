//?---------------------------------------------------------------
//?Copyright?(c)?Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//?---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Models.Students;

namespace OtripleS.Web.Api.Services.Foundations.Students
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<Student> RegisterStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudentOnRegister(student);

            return await this.storageBroker.InsertStudentAsync(student);
        });

        public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentId(studentId);
            Student storageStudent = await this.storageBroker.SelectStudentByIdAsync(studentId);
            ValidateStorageStudent(storageStudent, studentId);

            return storageStudent;
        });

        public ValueTask<Student> ModifyStudentAsync(Student student) =>
        TryCatch(async () =>
        {
            ValidateStudentOnModify(student);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(student.Id);

            ValidateStorageStudent(maybeStudent, student.Id);
            ValidateAgainstStorageStudentOnModify(inputStudent: student, storageStudent: maybeStudent);

            return await this.storageBroker.UpdateStudentAsync(student);
        });

        public ValueTask<Student> RemoveStudentByIdAsync(Guid studentId) =>
        TryCatch(async () =>
        {
            ValidateStudentId(studentId);

            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        });

        public IQueryable<Student> RetrieveAllStudents() =>
        TryCatch(() =>
        {
            IQueryable<Student> storageStudents = this.storageBroker.SelectAllStudents();
            ValidateStorageStudents(storageStudents);

            return storageStudents;
        });
    }
}
