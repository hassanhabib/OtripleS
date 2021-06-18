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
using OtripleS.Web.Api.Brokers.Storages;
using OtripleS.Web.Api.Brokers.UserManagement;
using OtripleS.Web.Api.Models.Foundations.Users;
using OtripleS.Web.Api.Services.Foundations.AssignmentAttachments;
using OtripleS.Web.Api.Services.Foundations.Assignments;
using OtripleS.Web.Api.Services.Foundations.Attachments;
using OtripleS.Web.Api.Services.Foundations.Attendances;
using OtripleS.Web.Api.Services.Foundations.CalendarEntries;
using OtripleS.Web.Api.Services.Foundations.CalendarEntryAttachments;
using OtripleS.Web.Api.Services.Foundations.Calendars;
using OtripleS.Web.Api.Services.Foundations.Classrooms;
using OtripleS.Web.Api.Services.Foundations.Contacts;
using OtripleS.Web.Api.Services.Foundations.CourseAttachments;
using OtripleS.Web.Api.Services.Foundations.Courses;
using OtripleS.Web.Api.Services.Foundations.ExamAttachments;
using OtripleS.Web.Api.Services.Foundations.ExamFees;
using OtripleS.Web.Api.Services.Foundations.Exams;
using OtripleS.Web.Api.Services.Foundations.Fees;
using OtripleS.Web.Api.Services.Foundations.GuardianAttachments;
using OtripleS.Web.Api.Services.Foundations.GuardianContacts;
using OtripleS.Web.Api.Services.Foundations.Guardians;
using OtripleS.Web.Api.Services.Foundations.Registrations;
using OtripleS.Web.Api.Services.Foundations.SemesterCourses;
using OtripleS.Web.Api.Services.Foundations.StudentAttachments;
using OtripleS.Web.Api.Services.Foundations.StudentContacts;
using OtripleS.Web.Api.Services.Foundations.StudentExamFees;
using OtripleS.Web.Api.Services.Foundations.StudentExams;
using OtripleS.Web.Api.Services.Foundations.StudentGuardians;
using OtripleS.Web.Api.Services.Foundations.StudentRegistrations;
using OtripleS.Web.Api.Services.Foundations.Students;
using OtripleS.Web.Api.Services.Foundations.StudentSemesterCourses;
using OtripleS.Web.Api.Services.Foundations.TeacherAttachments;
using OtripleS.Web.Api.Services.Foundations.TeacherContacts;
using OtripleS.Web.Api.Services.Foundations.Teachers;
using OtripleS.Web.Api.Services.Foundations.UserContacts;
using OtripleS.Web.Api.Services.Foundations.Users;

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
            AddNewtonSoftJson(services);
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
            services.AddTransient<ICalendarEntryService, CalendarEntryService>();
            services.AddTransient<IAttachmentService, AttachmentService>();
            services.AddTransient<IStudentAttachmentService, StudentAttachmentService>();
            services.AddTransient<IGuardianAttachmentService, GuardianAttachmentService>();
            services.AddTransient<ITeacherAttachmentService, TeacherAttachmentService>();
            services.AddTransient<ICalendarEntryAttachmentService, CalendarEntryAttachmentService>();
            services.AddTransient<ICourseAttachmentService, CourseAttachmentService>();
            services.AddTransient<IExamAttachmentService, ExamAttachmentService>();
            services.AddTransient<IAssignmentAttachmentService, AssignmentAttachmentService>();
            services.AddTransient<IFeeService, FeeService>();
            services.AddTransient<IExamFeeService, ExamFeeService>();
            services.AddTransient<IStudentExamFeeService, StudentExamFeeService>();
            services.AddTransient<IRegistrationService, RegistrationService>();
            services.AddTransient<IStudentRegistrationService, StudentRegistrationService>();

            services.AddIdentityCore<User>()
                    .AddRoles<Role>()
                    .AddEntityFrameworkStores<StorageBroker>()
                    .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder applicationBuilder, IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();
            }

            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseRouting();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private static void AddNewtonSoftJson(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });
        }
    }
}
