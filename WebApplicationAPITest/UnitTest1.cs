using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplicationAPITest
{
    public class UnitTest1
    {
        private readonly HttpClient _client;

        public UnitTest1()
        {
            _client = new HttpClient { BaseAddress = new Uri("https://localhost:7231/api/") };
        }

        [Fact]
        public async Task CreateLanguage_ShouldAdd100Languages()
        {
            Test_DeleteAllLanguageItems();

            for (int i = 1; i <= 100; i++)
            {
                string json = $"{{ \"Name\": \"Language {i}\", \"Description\": \"Description {i}\", \"LenghtOfCourse\": 5 }}";

                var response = await _client.PostAsync("Language",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                Assert.Equal("Удачное добавление", responseContent);
            }
        }

        [Fact]
        public async Task CreateLanguage_ShouldAdd100000Languages()
        {
            Test_DeleteAllLanguageItems();

            for (int i = 1; i <= 100000; i++)
            {
                string json = $"{{ \"Name\": \"Language {i}\", \"Description\": \"Description {i}\", \"LenghtOfCourse\": 5 }}";

                var response = await _client.PostAsync("Language",
                    new StringContent(json, Encoding.UTF8, "application/json"));

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var responseContent = await response.Content.ReadAsStringAsync();
                Assert.Equal("Удачное добавление", responseContent);
            }
        }

        [Fact]
        public async Task Test_DeleteAllLanguageItems()
        {
            var response = await _client.DeleteAsync("Language/deleteAll");

            Assert.True(response.IsSuccessStatusCode);

            var countResponse = await _client.GetAsync("Language");
            var languages = await countResponse.Content.ReadFromJsonAsync<List<LanguageItem>>();

            Assert.Empty(languages ?? []);
        }

    }
}