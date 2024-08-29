using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService.Services
{
    public class APIService
    {
        private readonly HttpClient _httpClient;
        public APIService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetData(CancellationToken stoppingToken)
        {
            var response = await _httpClient.GetAsync("https://reqres.in/api/users?page=1", stoppingToken);
            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
        public User ParseJson(JsonElement user)
        {
           
            int id = user.GetProperty("id").GetInt32();
            string? email = user.GetProperty("email").GetString();
            string? firstName = user.GetProperty("first_name").GetString();
            string? lastName = user.GetProperty("last_name").GetString();
            string? avatar = user.GetProperty("avatar").GetString();
            User userResponse = new User() { 
                UserId = id, 
                Email = email!, 
                FirstName = firstName!, 
                LastName = lastName!, 
                Avatar = avatar!
            };
         
            return userResponse;
        }


    

    }
}
