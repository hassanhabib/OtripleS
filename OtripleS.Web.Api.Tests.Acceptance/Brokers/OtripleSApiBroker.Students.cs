﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using OtripleS.Web.Api.Models.Students;
using System;
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
	}
}
