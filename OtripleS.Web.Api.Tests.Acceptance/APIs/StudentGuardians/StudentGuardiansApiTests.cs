// ---------------------------------------------------------------
//  Copyright (c) Coalition of the Good-Hearted Engineers 
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR 
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Guardians;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentGuardians;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentGuardians
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentGuardiansApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentGuardiansApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<StudentGuardian> CreateRandomStudentGuardian()
        {
            Student persistedStudent = await PostStudentAsync();
            Guardian persistedGuardian = await PostGuardianAsync();

            StudentGuardian randomStudentGuardian = CreateRandomStudentGuardianFiller(
                persistedStudent.Id,
                persistedGuardian.Id).Create();

            return randomStudentGuardian;
        }

        private async Task<StudentGuardian> PostStudentGuardianAsync()
        {
            StudentGuardian randomStudentGuardian = await CreateRandomStudentGuardian();
            await this.otripleSApiBroker.PostStudentGuardianAsync(randomStudentGuardian);

            return randomStudentGuardian;
        }

        private async ValueTask<StudentGuardian> DeleteStudentGuardianAsync(StudentGuardian studentGuardian)
        {
            StudentGuardian deletedStudentGuardian =
                await this.otripleSApiBroker.DeleteStudentGuardianAsync(
                    studentGuardian.StudentId,
                    studentGuardian.GuardianId);

            await this.otripleSApiBroker.DeleteGuardianByIdAsync(studentGuardian.GuardianId);
            await this.otripleSApiBroker.DeleteStudentByIdAsync(studentGuardian.StudentId);

            return deletedStudentGuardian;
        }

        private static Student CreateRandomStudent() =>
            CreateRandomStudentFiller().Create();

        private static Guardian CreateRandomGuardian() =>
            CreateRandomGuardianFiller().Create();

        private async ValueTask<Student> PostStudentAsync()
        {
            Student randomStudent = CreateRandomStudent();
            Student inputStudent = randomStudent;

            return await this.otripleSApiBroker.PostStudentAsync(inputStudent);
        }

        private async ValueTask<Guardian> PostGuardianAsync()
        {
            Guardian randomGuardian = CreateRandomGuardian();
            Guardian inputGuardian = randomGuardian;

            return await this.otripleSApiBroker.PostGuardianAsync(inputGuardian);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 5).GetValue();

        private static Filler<StudentGuardian> CreateRandomStudentGuardianFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<StudentGuardian>();

            filler.Setup()
                .OnProperty(studentGuardian => studentGuardian.CreatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.UpdatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.CreatedDate).Use(now)
                .OnProperty(studentGuardian => studentGuardian.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<StudentGuardian> CreateRandomStudentGuardianFiller(Guid studentId, Guid guardianId)
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<StudentGuardian>();

            filler.Setup()
                .OnProperty(studentGuardian => studentGuardian.CreatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.UpdatedBy).Use(posterId)
                .OnProperty(studentGuardian => studentGuardian.CreatedDate).Use(now)
                .OnProperty(studentGuardian => studentGuardian.UpdatedDate).Use(now)
                .OnProperty(studentGuardian => studentGuardian.StudentId).Use(studentId)
                .OnProperty(studentGuardian => studentGuardian.GuardianId).Use(guardianId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Student> CreateRandomStudentFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(posterId)
                .OnProperty(student => student.UpdatedBy).Use(posterId)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Guardian> CreateRandomGuardianFiller()
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<Guardian>();

            filler.Setup()
                .OnProperty(guardian => guardian.CreatedBy).Use(posterId)
                .OnProperty(guardian => guardian.UpdatedBy).Use(posterId)
                .OnProperty(guardian => guardian.CreatedDate).Use(now)
                .OnProperty(guardian => guardian.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
