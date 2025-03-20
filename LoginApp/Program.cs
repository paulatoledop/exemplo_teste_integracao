using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Redireciona a URL raiz para /login
app.MapGet("/", context =>
{
    context.Response.Redirect("/login");
    return Task.CompletedTask;
});

// "Banco de dados" em memória com os usuários
var users = new Dictionary<string, string>
{
    { "testuser", "secret" }
};

// GET /login: retorna o formulário de login
app.MapGet("/login", async context =>
{
    string html = @"
    <html>
      <head><title>Login</title></head>
      <body>
        <form method='post' action='/login'>
          <input type='text' name='username' placeholder='Username' required />
          <input type='password' name='password' placeholder='Password' required />
          <button type='submit'>Login</button>
        </form>
      </body>
    </html>";
    await context.Response.WriteAsync(html);
});

// POST /login: processa o login
app.MapPost("/login", async context =>
{
    var form = await context.Request.ReadFormAsync();
    var username = form["username"];
    var password = form["password"];

    if (users.ContainsKey(username) && users[username] == password)
    {
        await context.Response.WriteAsync($"Welcome, {username}!");
    }
    else
    {
        await context.Response.WriteAsync("Invalid username or password");
    }
});

app.Run();

// Torna a classe Program pública para que o projeto de testes possa referenciá-la.
public partial class Program { }
