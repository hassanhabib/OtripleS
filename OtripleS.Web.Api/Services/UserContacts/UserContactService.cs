//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Brokers.Loggings;
using OtripleS.Web.Api.Brokers.Storage;
using OtripleS.Web.Api.Models.UserContacts;

namespace OtripleS.Web.Api.Services.UserContacts
{
    public partial class UserContactService : IUserContactService
	{
		private readonly IStorageBroker storageBroker;
		private readonly ILoggingBroker loggingBroker;

		public UserContactService(
			IStorageBroker storageBroker,
			ILoggingBroker loggingBroker)
		{
			this.storageBroker = storageBroker;
			this.loggingBroker = loggingBroker;
		}

        public async ValueTask<UserContact> AddUserContactAsync(UserContact userContact)
        {
			return await this.storageBroker.InsertUserContactAsync(userContact);
		}
    }
}