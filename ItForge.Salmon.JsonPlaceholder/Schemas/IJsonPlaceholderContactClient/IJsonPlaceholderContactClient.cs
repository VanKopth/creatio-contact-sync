using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ItForge.Salmon.JsonPlaceholder
{
  public interface IJsonPlaceholderContactClient
  {
    Task<List<ContactDTO>> GetContactsAsync();
  }
}