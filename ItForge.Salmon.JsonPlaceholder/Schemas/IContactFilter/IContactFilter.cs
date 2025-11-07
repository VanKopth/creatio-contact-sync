using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItForge.Salmon.JsonPlaceholder
{
    public interface IContactFilter
    {
        List<ContactDTO> Filter(List<ContactDTO> Contacts);
    }
}

