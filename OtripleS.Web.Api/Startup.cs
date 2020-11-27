// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Brokers.UserManagement;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Services.Assignments;
using OtripleS.Web.Api.Services.Attendances;
using OtripleS.Web.Api.Services.Calendars;
using OtripleS.Web.Api.Services.Classrooms;
using OtripleS.Web.Api.Services.Contacts;
using OtripleS.Web.Api.Services.Courses;
using OtripleS.Web.Api.Services.Exams;
using OtripleS.Web.Api.Services.GuardianContacts;
using OtripleS.Web.Api.Services.Guardians;
using OtripleS.Web.Api.Services.SemesterCourses;
using OtripleS.Web.Api.Services.StudentContacts;
using OtripleS.Web.Api.Services.StudentExams;
using OtripleS.Web.Api.Services.StudentGuardians;
using OtripleS.Web.Api.Services.Students;
using OtripleS.Web.Api.Services.StudentSemesterCourses;
using OtripleS.Web.Api.Services.TeacherContacts;
using OtripleS.Web.Api.Services.Teachers;
using OtripleS.Web.Api.Services.UserContacts;
using OtripleS.Web.Api.Services.Users;

namespace OtripleS.Web.Api
{
    public class Startup
	{
		public Startup(IConfiguration configuration) =>
			Configuration = configuration;

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddMvc().AddNewtonsoftJson(options =>
			{
				options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
				options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
				options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			});

			services.AddDbContext<StorageBroker>();
			services.AddScoped<IUserManagementBroker, UserManagementBroker>();
			services.AddScoped<IStorageBroker, StorageBroker>();
			services.AddTransient<ILogger, Logger<LoggingBroker>>();
			services.AddTransient<ILoggingBroker, LoggingBroker>();
			services.AddTransient<IDateTimeBroker, DateTimeBroker>();
			services.AddTransient<IStudentService, StudentService>();
			services.AddTransient<ITeacherService, TeacherService>();
			services.AddTransient<ICourseService, CourseService>();
			services.AddTransient<IClassroomService, ClassroomService>();
			services.AddTransient<IAssignmentService, AssignmentService>();
			services.AddTransient<ISemesterCourseService, SemesterCourseService>();
			services.AddTransient<IStudentSemesterCourseService, StudentSemesterCourseService>();
			services.AddTransient<IAttendanceService, AttendanceService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IGuardianService, GuardianService>();
			services.AddTransient<IStudentGuardianService, StudentGuardianService>();
			services.AddTransient<IContactService, ContactService>();
			services.AddTransient<IStudentContactService, StudentContactService>();
			services.AddTransient<ITeacherContactService, TeacherContactService>();
			services.AddTransient<IGuardianContactService, GuardianContactService>();
			services.AddTransient<IUserContactService, UserContactService>();
			services.AddTransient<IExamService, ExamService>();
			services.AddTransient<IStudentExamService, StudentExamService>();
			services.AddTransient<ICalendarService, CalendarService>();

			services.AddIdentityCore<User>()
					.AddRoles<Role>()
					.AddEntityFrameworkStores<StorageBroker>()
					.AddDefaultTokenProviders();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
