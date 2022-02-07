﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Microsoft.Data.SqlClient;
using Moq;
using OtripleS.Web.Api.Brokers.DateTimes;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.UserManagement;
using OtripleS.Web.Api.Models.Users;
using OtripleS.Web.Api.Services.Foundations.Users;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Users
{
    public partial class UserServiceTests
    {
        private readonly Mock<IUserManagementBroker> userManagementBrokerMock;
        private readonly Mock<IDateTimeBroker> dateTimeBrokerMock;
        private readonly Mock<ILoggingBroker> loggingBrokerMock;
        private readonly IUserService userService;

        public UserServiceTests()
        {
            this.userManagementBrokerMock = new Mock<IUserManagementBroker>();
            this.dateTimeBrokerMock = new Mock<IDateTimeBroker>();
            this.loggingBrokerMock = new Mock<ILoggingBroker>();

            this.userService = new UserService(
                userManagementBroker: this.userManagementBrokerMock.Object,
                dateTimeBroker: this.dateTimeBrokerMock.Object,
                loggingBroker: this.loggingBrokerMock.Object);
        }

        private static User CreateRandomUser()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = new Filler<EmailAddresses>().Create().ToString(),
                Name = new Filler<RealNames>().Create().ToString(),
                FamilyName = new Filler<RealNames>().Create().ToString(),
                CreatedDate = DateTimeOffset.UtcNow,
                UpdatedDate = DateTimeOffset.UtcNow

            };

            return user;
        }

        private static string GetRandomPassword() => new MnemonicString(1, 8, 20).GetValue();

        private static Expression<Func<Exception, bool>> SameExceptionAs(Exception expectedException)
        {
            return actualException =>
                actualException.Message == expectedException.Message
                && actualException.InnerException.Message == expectedException.InnerException.Message;
        }

        private static DateTimeOffset GetRandomDateTime() =>
            new DateTimeRange(earliestDate: new DateTime()).GetValue();

        public static IEnumerable<object[]> InvalidMinuteCases()
        {
            int randomMoreThanMinuteFromNow = GetRandomNumber();
            int randomMoreThanMinuteBeforeNow = GetNegativeRandomNumber();

            return new List<object[]>
            {
                new object[] { randomMoreThanMinuteFromNow },
                new object[] { randomMoreThanMinuteBeforeNow }
            };
        }

        private static int GetRandomNumber() => new IntRange(min: 2, max: 90).GetValue();
        private static int GetNegativeRandomNumber() => -1 * GetRandomNumber();

        private static User CreateRandomUser(DateTimeOffset dates)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = new Filler<EmailAddresses>().Create().ToString(),
                Name = new Filler<RealNames>().Create().ToString(),
                FamilyName = new Filler<RealNames>().Create().ToString(),
                CreatedDate = dates,
                UpdatedDate = dates
            };

            return user;
        }

        private static string GetRandomMessage() => new MnemonicString().GetValue();
        private static SqlException GetSqlException() =>
            (SqlException)FormatterServices.GetUninitializedObject(typeof(SqlException));

        private static IQueryable<User> CreateRandomUsers(DateTimeOffset dates)
        {
            var users = new List<User>();
            for (int i = 0; i < GetRandomNumber(); i++)
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = new Filler<EmailAddresses>().Create().ToString(),
                    Name = new Filler<RealNames>().Create().ToString(),
                    FamilyName = new Filler<RealNames>().Create().ToString(),
                    CreatedDate = dates,
                    UpdatedDate = dates
                };

                users.Add(user);
            }

            return users.AsQueryable();
        }
    }
}
