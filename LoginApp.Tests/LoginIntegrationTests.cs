using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LoginApp.Tests
{
    public class LoginIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public LoginIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task SuccessfulLogin_ReturnsWelcomeMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "username", "testuser" },
                { "password", "secret" }
            };

            // Act
            var response = await _client.PostAsync("/login", new FormUrlEncodedContent(formData));
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Welcome, testuser", content);
        }

        [Fact]
        public async Task UnsuccessfulLogin_ReturnsErrorMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                { "username", "testuser" },
                { "password", "wrongpassword" }
            };

            // Act
            var response = await _client.PostAsync("/login", new FormUrlEncodedContent(formData));
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid username or password", content);
        }
        
        // Exemplo de método auxiliar para reduzir repetição
        private async Task<string> PostLoginFormAsync(Dictionary<string, string> formData)
        {
            var response = await _client.PostAsync("/login", new FormUrlEncodedContent(formData));
            // Aqui não se usa EnsureSuccessStatusCode pois pode ser teste de falha
            return await response.Content.ReadAsStringAsync();
        }
    }
}
