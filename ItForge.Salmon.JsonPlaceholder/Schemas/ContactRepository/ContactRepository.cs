using System;
using System.Linq;
using System.Threading.Tasks;
using Terrasoft.Core;
using Terrasoft.Core.Entities;

namespace ItForge.Salmon.JsonPlaceholder
{
    public class ContactRepository : IContactRepository
    {
        private readonly UserConnection _userConnection;

        public ContactRepository(UserConnection userConnection)
        {
            _userConnection = userConnection ?? throw new ArgumentNullException(nameof(userConnection));
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "Contact");
            esq.AddColumn("Id");

            var emailFilter = esq.CreateFilterWithParameters(
                FilterComparisonType.Equal,
                "Email",
                email
            );
            esq.Filters.Add(emailFilter);

            var entities = esq.GetEntityCollection(_userConnection);
            return entities.Count > 0;
        }

        public async Task UpdateContactAsync(string email, string name, string phone)
        {
            var esq = new EntitySchemaQuery(_userConnection.EntitySchemaManager, "Contact");
            esq.AddAllSchemaColumns();

            var emailFilter = esq.CreateFilterWithParameters(
                FilterComparisonType.Equal,
                "Email",
                email
            );
            esq.Filters.Add(emailFilter);

            var entities = esq.GetEntityCollection(_userConnection);

            if (entities.Count > 0)
            {
                var contact = entities.First();
                contact.SetColumnValue("Name", name);

                if (!string.IsNullOrEmpty(phone))
                {
                    contact.SetColumnValue("Phone", phone);
                }

                contact.Save();
            }
        }

        public async Task InsertContactAsync(string email, string name, string phone)
        {
            var schema = _userConnection.EntitySchemaManager.GetInstanceByName("Contact");
            var entity = schema.CreateEntity(_userConnection);

            entity.SetDefColumnValues();
            entity.SetColumnValue("Name", name);
            entity.SetColumnValue("Email", email);
            entity.SetColumnValue("Phone", phone);

            entity.Save();
        }
    }
}
