using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Xunit;
using Application.Services;
using System.Security.Claims;

namespace Aplication.Tests
{
    public class AuthServiceTests
    {
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // Configuração fake para testes
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:SecretKey", "chave-super-secreta-para-testes-256bits" },
                { "Jwt:Issuer", "GestaoAutoRepara" },
                { "Jwt:Audience", "GestaoAutoReparaUsers" },
                { "Jwt:ExpirationMinutes", "5" }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _authService = new AuthService(configuration);
        }

        [Fact]
        public void ValidarCredenciais_CredenciaisValidas_DeveRetornarTrue()
        {
            var result = _authService.ValidarCredenciais("admin", "admin123");
            Assert.True(result);
        }

        [Fact]
        public void ValidarCredenciais_CredenciaisInvalidas_DeveRetornarFalse()
        {
            var result = _authService.ValidarCredenciais("user", "senhaErrada");
            Assert.False(result);
        }

      [Fact]
        public void GerarToken_DeveGerarTokenValido()
        {
            var token = _authService.GerarToken("usuarioTeste");

            Assert.False(string.IsNullOrEmpty(token));

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("GestaoAutoRepara", jwtToken.Issuer);
            Assert.Equal("GestaoAutoReparaUsers", jwtToken.Audiences.First());

            // Correção: verificar os claims com os tipos corretos
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.NameIdentifier && c.Value == "usuarioTeste");
            Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Name && c.Value == "usuarioTeste");
        }
        [Fact]
        public void GerarToken_DeveExpirarNoTempoCorreto()
        {
            var token = _authService.GerarToken("usuarioTeste");

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var tempoExpiracao = jwtToken.ValidTo;
            var diferenca = tempoExpiracao - DateTime.UtcNow;

            Assert.True(diferenca.TotalMinutes <= 5.1 && diferenca.TotalMinutes >= 4.5);
        }
    }
}
