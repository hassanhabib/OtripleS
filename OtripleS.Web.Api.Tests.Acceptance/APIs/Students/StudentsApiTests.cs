using System;
using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Tests.Acceptance.Brokers;
using Tynamix.ObjectFiller;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Students
{
	[Collection(nameof(ApiTestCollection))]
	public class StudentsApiTests
	{
		private readonly OtripleSApiBroker otripleSApiBroker;
		private readonly DateTimeBroker dateTimeBroker;

		public StudentsApiTests(OtripleSApiBroker otripleSApiBroker)
		{
			this.dateTimeBroker = new DateTimeBroker();
			this.otripleSApiBroker = otripleSApiBroker;
		}

		private Student CreateRandomStudent()
		{
			var filler = new Filler<Student>();
			filler.Setup().OnType<DateTimeOffset>().Use(GetRandomDateTime());
			return filler.Create();
		}

		private static DateTimeOffset GetRandomDateTime() =>
		 new DateTimeRange(earliestDate: new DateTime()).GetValue();

		[Fact]
		public async Task ShouldPostStudentAsync()
		{
			// given
			Student randomStudent = CreateRandomStudent();
			Student inputStudent = randomStudent;
			Student expectedStudent = inputStudent;

			// when 
			await this.otripleSApiBroker.PostStudentAsync(inputStudent);

			Student actualStudent =
				await this.otripleSApiBroker.GetStudentByIdAsync(inputStudent.Id);

			// then
			actualStudent.Should().BeEquivalentTo(expectedStudent);

			await this.otripleSApiBroker.DeleteStudentByIdAsync(actualStudent.Id);
		}

		[Fact]
		public async Task ShouldPutStudentAsync()
		{
			//given
			Student randomStudent = CreateRandomStudent();
			await this.otripleSApiBroker.PostStudentAsync(randomStudent);
			Student newStudent = CreateRandomStudent();

			//when
			await this.otripleSApiBroker.PutStudentAsync(randomStudent.Id, newStudent);

			Student actualStudent =
				await this.otripleSApiBroker.GetStudentByIdAsync(randomStudent.Id);

			//then
			actualStudent.Should().BeEquivalentTo(newStudent);

			await this.otripleSApiBroker.DeleteStudentByIdAsync(randomStudent.Id);
		}

	}
}
