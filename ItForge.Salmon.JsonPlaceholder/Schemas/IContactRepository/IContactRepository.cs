using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItForge.Salmon.JsonPlaceholder
{ 
    public interface IContactRepository
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task UpdateContactAsync(string email, string name, string phone);
        Task InsertContactAsync(string email, string name, string phone);
    }
}