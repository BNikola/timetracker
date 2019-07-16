using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimeTrackerApp.Tests.IntegrationTests
{
    public class UsersApiTests
    {
        private readonly HttpClient _client;
        private readonly string _nonAdminToken;
        private readonly string _adminToken;

        public UsersApiTests()
        {
            const string issuer = "http://localhost:44397";
            const string key = "asdfmkafksdlfklnmkkasnkasvndndkndfkanjdfa";

            // kreiranje servera
            var server = new TestServer(new WebHostBuilder()
                .UseSetting("Tokens:Issuer", issuer)
                .UseSetting("Tokens:Key", key)
                .UseSetting("ConnectionStrings:DefaultConnection", "DataSource=:memory:")       // sqlite - koristi svoju in memory implementaciju baze
                .UseStartup<Startup>()
                .UseUrls("https://localhost:44397"))
            {
                BaseAddress = new Uri("https://localhost:44397")
            };

            _client = server.CreateClient();

            _nonAdminToken = JwtTokenGenerator.Generate(
                "etf-workshop", false, issuer, key);
            _adminToken = JwtTokenGenerator.Generate(
                "etf-workshop", true, issuer, key);
        }

        // delete testovi
        // bez header-a
        [Fact]
        public async Task Delete_NoAuthorizatioinHeader_ReturnsUnauthorized()
        {
            // Arrange
            _client.DefaultRequestHeaders.Clear();      // da nema header-a

            // Act
            var result = await _client.DeleteAsync("/api/users/1");

            Assert.Equal(HttpStatusCode.Unauthorized, result.StatusCode);

        }

        // sa header-om
        [Fact]
        public async Task Delete_NotAdmin_ReturnsForbidden()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_nonAdminToken}" });    // dodavanje header-a (nije admin)

            var result = await _client.DeleteAsync("/api/users/1");

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task Delete_NoId_ReturnsMethodNotAllowed()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/users/ ");

            Assert.Equal(HttpStatusCode.MethodNotAllowed, result.StatusCode);
        }
        
        // puca zbog bug-a u entity framework-u (EF)
        [Fact]
        public async Task Delete_NonExistingId_ReturnsNotFound()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/users/0");

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Delete_ExistingId_ReturnsOk()
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders
                .Add("Authorization", new[] { $"Bearer {_adminToken}" });

            var result = await _client.DeleteAsync("/api/users/1");

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }
    }

}
