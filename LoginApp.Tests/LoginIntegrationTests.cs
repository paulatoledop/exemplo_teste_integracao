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

            // Act client
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
// Este código define uma classe de testes de integração para a aplicação de login usando xUnit e o recurso de WebApplicationFactory do ASP.NET Core. A classe cria um HttpClient para simular 
// requisições HTTP reais aos endpoints da aplicação e contém dois testes:
	// •	SuccessfulLogin_ReturnsWelcomeMessage:
//              Envia uma requisição POST para o endpoint /login com credenciais válidas (usuário “testuser” e senha “secret”) e verifica se a resposta contém a mensagem de boas-vindas.
//  	•	UnsuccessfulLogin_ReturnsErrorMessage:
//              Envia uma requisição POST com credenciais inválidas e verifica se a resposta retorna a mensagem de erro correspondente.

//  Há também um método auxiliar para centralizar o envio de requisições e evitar repetição de código.

// Este teste é considerado de integração porque ele avalia o funcionamento conjunto de várias partes do sistema: 
// o pipeline HTTP, os endpoints configurados e a lógica de autenticação implementada na aplicação.
//  Em vez de testar componentes isolados (como um teste unitário faria), ele simula o comportamento real de uma requisição HTTP, 
// garantindo que os diferentes módulos da aplicação funcionem corretamente em conjunto.