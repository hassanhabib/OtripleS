// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Moq;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.UserContacts;
using OtripleS.Web.Api.Services.UserContacts;
using Tynamix.ObjectFiller;

namespace OtripleS.Web.Api.Tests.Unit.Services.UserContacts
{
	public partial class UserContactServiceTests
	{
		private readonly Mock<IStorageBroker> storageBrokerMock;
		private readonly Mock<ILoggingBroker> loggingBrokerMock;
		private readonly IUserContactService userContactService;

		public UserContactServiceTests()
		{
			this.storageBrokerMock = new Mock<IStorageBroker>();
			this.loggingBrokerMock = new Mock<ILoggingBroker>();

			this.userContactService = new UserContactService(
				storageBroker: this.storageBrokerMock.Object,
				loggingBroker: this.loggingBrokerMock.Object);
		}

		private UserContact CreateRandomUserContact() =>
			CreateUserContactFiller().Create();

		private static Filler<UserContact> CreateUserContactFiller()
		{
			var filler = new Filler<UserContact>();
			filler.Setup()
				.OnProperty(usercontact => usercontact.User).IgnoreIt()
				.OnProperty(usercontact => usercontact.Contact).IgnoreIt();

			return filler;
		}
	}
}
