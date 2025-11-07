using System;
using System.Collections.Generic;
using System.Linq;

namespace ItForge.Salmon.JsonPlaceholder
{
    public class NameContainsLetterFilter : IContactFilter
    {   
        public List<ContactDTO> Filter(List<ContactDTO> Contacts)
        {
            if (Contacts == null)
            {
                return new List<ContactDTO>();
            }
            
            List<ContactDTO> filteredContacts = Contacts
                .Where(c => !string.IsNullOrEmpty(c.Name) && c.Name.IndexOf("A", StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            
            return filteredContacts;
        }
    }
}