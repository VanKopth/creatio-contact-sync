using System;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Newtonsoft.Json;
using Terrasoft.Core;
using Terrasoft.Core.DB;
using Terrasoft.Core.Factories;
using Terrasoft.Web.Common;

namespace ItForge.Salmon.JsonPlaceholder
{
  [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ContactSyncExecutor : BaseService
    {      
        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        public async void ExecuteSync()
        {
            var _userConnection = UserConnection;
            try
            {
                var apiClient = new JsonPlaceholderContactClient();
                var ContactFilter = new NameContainsLetterFilter();
                var contactRepository = new ContactRepository(_userConnection);
                var phoneGenerator = new PhoneGenerator();
                
                var syncService = new JsonPlaceholderService(
                    apiClient,
                    ContactFilter,
                    contactRepository,
                    phoneGenerator
                );
                
                await syncService.SyncContactsAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Filed to execute", e);
            }
        }
    }
}
