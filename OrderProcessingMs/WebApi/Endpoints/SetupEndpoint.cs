using Core.Controllers;
using Core.Dtos;
using Core.Enums;
using Core.Interfaces;
using Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints
{
    [AllowAnonymous]
    [ApiController]
    [Route("Setup")]
    public class SetupEndpoint : ControllerBase
    {
        private readonly IDbConnection _dbConnection;
        private readonly string _jwtSecret;

        public SetupEndpoint(IDbConnection dbConnection, IConfiguration configuration)
        {
            _dbConnection = dbConnection;

            string? jwtSecret = configuration["ApiAuthentication:Token"];

            if (string.IsNullOrEmpty(jwtSecret))
                throw new ArgumentException("A chave de autenticação da API não está configurada no appsettings.json.");

            _jwtSecret = jwtSecret;
        }
    }
}
