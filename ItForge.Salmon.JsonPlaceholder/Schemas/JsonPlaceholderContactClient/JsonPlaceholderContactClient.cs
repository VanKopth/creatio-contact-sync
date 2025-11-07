using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ItForge.Salmon.JsonPlaceholder
{
    public class JsonPlaceholderContactClient : IJsonPlaceholderContactClient
    {
       private const string BaseUrl = "https://jsonplaceholder.typicode.com";
        
        public async Task<List<ContactDTO>> GetContactsAsync()
        {
            string responseText = null;
            
            try
            {
                var request = (HttpWebRequest)WebRequest.Create($"{BaseUrl}/users");
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = 30000;
                
                using (var response = (HttpWebResponse)request.GetResponse())
                using (var streamReader = new StreamReader(response.GetResponseStream()))
                {
                    responseText = streamReader.ReadToEnd();
                }
                
                return JsonConvert.DeserializeObject<List<ContactDTO>>(responseText);
            }
            catch (WebException ex)
            {
                throw new Exception("Failed to fetch Contacts from JSONPlaceholder API", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to deserialize Contacts from JSONPlaceholder API", ex);
            }

            return new List<ContactDTO>();
        }
    }
}

