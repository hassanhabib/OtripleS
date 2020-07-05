//?---------------------------------------------------------------
//?Copyright?(c)?Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//?---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Extensions;
using OtripleS.Web.Api.Models;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Requests;

namespace OtripleS.Web.Api.Services
{
    public partial class StudentService : IStudentService
    {
        private readonly IStorageBroker storageBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentService(
            IStorageBroker storageBroker,
            ILoggingBroker loggingBroker)
        {
            this.storageBroker = storageBroker;
            this.loggingBroker = loggingBroker;
        }

        public async ValueTask<Student> DeleteStudentAsync(Guid studentId)
        {
            ValidateStudentId(studentId);
            Student maybeStudent =
                await this.storageBroker.SelectStudentByIdAsync(studentId);

            ValidateStorageStudent(maybeStudent, studentId);

            return await this.storageBroker.DeleteStudentAsync(maybeStudent);
        }

        public ValueTask<Student> ModifyStudentAsync(Guid studentId, StudentUpdateDto updateDto)
        {
            return TryCatch(async () =>
            {
                var student =
                    await this.storageBroker.SelectStudentByIdAsync(studentId);

                MapChangesToStudent(updateDto, student);
                ValidateStudent(student);

                return await this.storageBroker.UpdateStudentAsync(student);
            });
        }

        private static void MapChangesToStudent(StudentUpdateDto updateDto, Student student)
        {
            student.IdentityNumber = updateDto.IdentityNumber ?? student.IdentityNumber;
            student.FirstName = updateDto.FirstName ?? student.FirstName;
            student.MiddleName = updateDto.MiddleName ?? student.MiddleName;
            student.LastName = updateDto.LastName ?? student.LastName;
            student.BirthDate = updateDto.BirthDate.IsValid()
                ? updateDto.BirthDate
                : student.BirthDate;
            student.Gender = updateDto.Gender.HasValue()
                ? updateDto.Gender
                : student.Gender;
        }

        public ValueTask<Student> RetrieveStudentByIdAsync(Guid studentId) =>
            TryCatch(async () =>
            {
                ValidateStudentId(studentId);
                Student storageStudent = await this.storageBroker.SelectStudentByIdAsync(studentId);
                ValidateStorageStudent(storageStudent, studentId);

                return storageStudent;
            });
    }
}