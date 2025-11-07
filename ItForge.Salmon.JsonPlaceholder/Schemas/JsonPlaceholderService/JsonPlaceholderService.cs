using System;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;


namespace ItForge.Salmon.JsonPlaceholder
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class JsonPlaceholderService : BaseService
    {
        private readonly IJsonPlaceholderContactClient _apiClient;
        private readonly IContactFilter _contactFilter;
        private readonly IContactRepository _contactRepository;
        private readonly IPhoneGenerator _phoneGenerator;

        public JsonPlaceholderService(
            IJsonPlaceholderContactClient apiClient,
            IContactFilter contactFilter,
            IContactRepository contactRepository,
            IPhoneGenerator phoneGenerator)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _contactFilter = contactFilter ?? throw new ArgumentNullException(nameof(contactFilter));
            _contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
            _phoneGenerator = phoneGenerator ?? throw new ArgumentNullException(nameof(phoneGenerator));
        }
        public async Task SyncContactsAsync()
        {
            try
            {
                var contacts = await _apiClient.GetContactsAsync();

                if (contacts == null || contacts.Count == 0)
                {
                    return;
                }
                
                var filteredContacts = _contactFilter.Filter(contacts);

                if (filteredContacts.Count == 0)
                {
                    return;
                }

                foreach (var contact in filteredContacts)
                {
                    SyncSingleContactAsync(contact);
                }
            }
            catch (Exception e)
            {
                throw new Exception("failed to sync contacts", e);
            }
        }

        private async Task SyncSingleContactAsync(ContactDTO contact)
        {
            if (string.IsNullOrWhiteSpace(contact.Email))
            {
                return;
            }

            try
            {
                var exists = await _contactRepository.ExistsByEmailAsync(contact.Email);

                if (exists)
                {
                    await _contactRepository.UpdateContactAsync(contact.Email, contact.Name, null);
                }
                else
                {
                    var phoneNumber = _phoneGenerator.GeneratePhilippineNumber();

                    await _contactRepository.InsertContactAsync(contact.Email, contact.Name, phoneNumber);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to sync contact with email {contact.Email}", e);
            }
        }
    }
}
