// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

		[Fact]
		public async Task ShouldPutTeacherAsync()
		{
			// given
			Teacher randomTeacher = CreateRandomTeacher();
			await this.otripleSApiBroker.PostTeacherAsync(randomTeacher);
			Teacher modifiedTeacher = UpdateTeacherRandom(randomTeacher);

			// when
			await this.otripleSApiBroker.PutTeacherAsync(modifiedTeacher);

			Teacher actualTeacher =
				await this.otripleSApiBroker.GetTeacherByIdAsync(randomTeacher.Id);

			// then
			actualTeacher.Should().BeEquivalentTo(modifiedTeacher);

			await this.otripleSApiBroker.DeleteTeacherByIdAsync(actualTeacher.Id);
		}
	}
}
