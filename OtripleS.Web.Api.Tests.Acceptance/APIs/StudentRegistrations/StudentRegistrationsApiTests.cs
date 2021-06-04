// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using OtripleS.Web.Api.Tests.Acceptance.Models.Registrations;
using OtripleS.Web.Api.Tests.Acceptance.Models.StudentRegistrations;
using OtripleS.Web.Api.Tests.Acceptance.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Models.Users;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.StudentRegistrations
{
    [Collection(nameof(ApiTestCollection))]
    public partial class StudentRegistrationsApiTests
    {
        private readonly OtripleSApiBroker otripleSApiBroker;

        public StudentRegistrationsApiTests(OtripleSApiBroker otripleSApiBroker) =>
            this.otripleSApiBroker = otripleSApiBroker;

        private async Task<StudentRegistration> CreateRandomStudentRegistration()
        {
            User user = await PostRandomUserAsync();
            Student persistedStudent = await PostStudentAsync(user);
            Registration persistedRegistration = await PostRegistrationAsync(user);

            StudentRegistration randomStudentRegistration = CreateRandomStudentRegistrationFiller(
                persistedStudent.Id,
                persistedRegistration.Id).Create();

            return randomStudentRegistration;
        }

        private async Task<StudentRegistration> PostStudentRegistrationAsync()
        {
            StudentRegistration randomStudentRegistration = await CreateRandomStudentRegistration();
            await this.otripleSApiBroker.PostStudentRegistrationAsync(randomStudentRegistration);

            return randomStudentRegistration;
        }

        private async ValueTask<StudentRegistration> DeleteStudentRegistrationAsync(
            StudentRegistration studentRegistration)
        {
            StudentRegistration deletedStudentRegistration =
                await this.otripleSApiBroker.DeleteStudentRegistrationAsync(
                    studentRegistration.StudentId,
                    studentRegistration.RegistrationId);

            await this.otripleSApiBroker.DeleteRegistrationByIdAsync(studentRegistration.RegistrationId);
            
            Student deletedStudent = 
                await this.otripleSApiBroker.DeleteStudentByIdAsync(studentRegistration.StudentId);

            await this.otripleSApiBroker.DeleteUserByIdAsync(deletedStudent.CreatedBy);

            return deletedStudentRegistration;
        }

        private async ValueTask<User> PostRandomUserAsync()
        {
            User user = CreateRandomUser();

            return await this.otripleSApiBroker.PostUserAsync(user);
        }

        private static User CreateRandomUser() =>
            CreateRandomUserFiller().Create();

        private static Student CreateRandomStudent(User user) =>
            CreateRandomStudentFiller(user).Create();

        private static Registration CreateRandomRegistration(User user) =>
            CreateRandomRegistrationFiller(user).Create();

        private async ValueTask<Student> PostStudentAsync(User user)
        {
            Student randomStudent = CreateRandomStudent(user);
            Student inputStudent = randomStudent;

            return await this.otripleSApiBroker.PostStudentAsync(inputStudent);
        }

        private async ValueTask<Registration> PostRegistrationAsync(User user)
        {
            Registration randomRegistration = CreateRandomRegistration(user);
            Registration inputRegistration = randomRegistration;

            return await this.otripleSApiBroker.PostRegistrationAsync(inputRegistration);
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        private static int GetRandomNumber() => new IntRange(min: 1, max: 5).GetValue();

        private static Filler<User> CreateRandomUserFiller()
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset? nullableDateTime = null;
            var filler = new Filler<User>();

            filler.Setup()
                .OnProperty(user => user.CreatedDate).Use(now)
                .OnProperty(user => user.UpdatedDate).Use(now)
                .OnProperty(user => user.LockoutEnd).Use(nullableDateTime)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<StudentRegistration> CreateRandomStudentRegistrationFiller(
            Guid studentId, 
            Guid registrationId)
        {
            var now = DateTimeOffset.UtcNow;
            var posterId = Guid.NewGuid();
            var filler = new Filler<StudentRegistration>();

            filler.Setup()
                .OnProperty(studentRegistration => studentRegistration.StudentId).Use(studentId)
                .OnProperty(studentRegistration => studentRegistration.RegistrationId).Use(registrationId)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Student> CreateRandomStudentFiller(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<Student>();

            filler.Setup()
                .OnProperty(student => student.CreatedBy).Use(user.Id)
                .OnProperty(student => student.UpdatedBy).Use(user.Id)
                .OnProperty(student => student.CreatedDate).Use(now)
                .OnProperty(student => student.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }

        private static Filler<Registration> CreateRandomRegistrationFiller(User user)
        {
            var now = DateTimeOffset.UtcNow;
            var filler = new Filler<Registration>();

            filler.Setup()
                .OnProperty(registration => registration.CreatedBy).Use(user.Id)
                .OnProperty(registration => registration.UpdatedBy).Use(user.Id)
                .OnProperty(registration => registration.CreatedDate).Use(now)
                .OnProperty(registration => registration.UpdatedDate).Use(now)
                .OnType<DateTimeOffset>().Use(GetRandomDateTime());

            return filler;
        }
    }
}
