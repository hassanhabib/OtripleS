// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
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

		public ValueTask<UserContact> AddUserContactAsync(UserContact userContact)
		{
			throw new NotImplementedException();
		}

		public async ValueTask<UserContact> RemoveUserContactByIdAsync(Guid userId, Guid contactId)
		{
			UserContact mayBeUserContact =
				await this.storageBroker.SelectUserContactByIdAsync(userId, contactId);

			return await this.storageBroker.DeleteUserContactAsync(mayBeUserContact);
		}

		public IQueryable<UserContact> RetrieveAllUserContacts()
		{
			throw new NotImplementedException();
		}

		public ValueTask<UserContact> RetrieveUserContactByIdAsync(Guid userId, Guid contactId)
		{
			throw new NotImplementedException();
		}
	}
}
