using System.Threading.Tasks;
using FluentAssertions;
using OtripleS.Web.Api.Models.Teachers;
using Xunit;

namespace OtripleS.Web.Api.Tests.Acceptance.APIs.Teachers
{
	public partial class TeachersApiTests
	{
		[Fact]
		public async Task ShouldPostTeacherAsync()
		{
			// given
			Teacher randomTeacher = CreateRandomTeacher();
			Teacher inputTeacher = randomTeacher;
			Teacher expectedTeacher = inputTeacher;

			// when 
			await this.otripleSApiBroker.PostTeacherAsync(inputTeacher);

			Teacher actualTeacher =
				await this.otripleSApiBroker.GetTeacherByIdAsync(inputTeacher.Id);

			// then
			actualTeacher.Should().BeEquivalentTo(expectedTeacher);

			await this.otripleSApiBroker.DeleteTeacherByIdAsync(actualTeacher.Id);
		}

	}
}
