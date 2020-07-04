using OtripleS.Web.Api.Models.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OtripleS.Web.Api.Tests.Acceptance.Brokers
{
	public partial class OtripleSApiBroker
	{
		private const string StudentsRelativeUrl = "api/students";

		public async ValueTask<Student> PostStudentAsync(Student student) =>
			await this.apiFactoryClient.PostContentAsync(StudentsRelativeUrl, student);

		public async ValueTask<Student> GetStudentByIdAsync(Guid studentId) =>
			await this.apiFactoryClient.GetContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

		public async ValueTask<Student> DeleteStudentByIdAsync(Guid studentId) =>
			await this.apiFactoryClient.DeleteContentAsync<Student>($"{StudentsRelativeUrl}/{studentId}");

		public async ValueTask<Student> PutStudentAsync(Guid studentId, Student student) => 
			await this.apiFactoryClient.PutContentAsync($"{StudentsRelativeUrl}/{studentId}", student);
	}
}
