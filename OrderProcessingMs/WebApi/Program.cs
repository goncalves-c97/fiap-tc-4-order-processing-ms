using Core.Dtos;
using Core.Entities;
using Core.Gateways.Microservices;
using Core.Interfaces;
using Core.Interfaces.Gateways.Microservices;
using Core.Settings;
using Dapper;
using Infra.Data.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

string? authenticationKey = builder.Configuration["API_AUTHENTICATION_KEY"];

if (string.IsNullOrEmpty(authenticationKey))
    throw new KeyNotFoundException("Chave 'API_AUTHENTICATION_KEY' não encontrada.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationKey))
    };
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fast Food Challenge API - Order Processing microservice (V1.0)",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Carlos Henrique Bezerra Gonçalves",
            Email = "carlos_henrique97@outlook.com.br",
            Url = new Uri("https://www.linkedin.com/in/carlos-henrique-b-goncalves/")
        }
    });

    #region Recursos para mostrar enum no Swagger

    c.SchemaGeneratorOptions = new SchemaGeneratorOptions
    {
        UseAllOfForInheritance = true,
        UseAllOfToExtendReferenceSchemas = true
    };

    c.UseInlineDefinitionsForEnums();

    #endregion

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autorização JWT utilizando o padrão Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddScoped<IDbConnection>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var rawConnectionString = config["DB_CONNECTION_STRING"];
    var databaseName = config["DB_NAME"];

    if (string.IsNullOrWhiteSpace(rawConnectionString))
        throw new KeyNotFoundException("Chave 'DB_CONNECTION_STRING' não encontrada.");

    if (string.IsNullOrEmpty(databaseName))
        throw new KeyNotFoundException("Chave 'DB_NAME' não encontrada.");

    // Keep existing initializer call for compatibility.
    DatabaseInitializer.EnsureDatabaseExists(rawConnectionString, databaseName);

    SqlMapper.SetTypeMap(typeof(Acompanhamento), new SnakeCaseTypeMapper<Acompanhamento>());
    SqlMapper.SetTypeMap(typeof(AlteracaoIngredienteCombo), new SnakeCaseTypeMapper<AlteracaoIngredienteCombo>());
    SqlMapper.SetTypeMap(typeof(Bebida), new SnakeCaseTypeMapper<Bebida>());
    SqlMapper.SetTypeMap(typeof(Combo), new SnakeCaseTypeMapper<Combo>());
    SqlMapper.SetTypeMap(typeof(ComboPedido), new SnakeCaseTypeMapper<ComboPedido>());
    SqlMapper.SetTypeMap(typeof(Ingrediente), new SnakeCaseTypeMapper<Ingrediente>());
    SqlMapper.SetTypeMap(typeof(IngredientesLanche), new SnakeCaseTypeMapper<IngredientesLanche>());
    SqlMapper.SetTypeMap(typeof(Lanche), new SnakeCaseTypeMapper<Lanche>());
    SqlMapper.SetTypeMap(typeof(Sobremesa), new SnakeCaseTypeMapper<Sobremesa>());

    var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(rawConnectionString)
    {
        InitialCatalog = databaseName
    };

    var finalConnectionString = builder.ToString();

    return new SqlServerConnection(finalConnectionString);
});

builder.Configuration.AddEnvironmentVariables();

builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("APP_SETTINGS"));

builder.Services.Configure<EmailSettingsDto>(builder.Configuration.GetSection("EMAIL_SETTINGS"));

#region Configuração dos microsserviços

builder.Services.AddHttpClient<ILoginMsGateway, LoginMsGateway>(client =>
{
    string? loginMicroserviceUrl = builder.Configuration["LOGIN_MS_URL"];

    if (string.IsNullOrEmpty(loginMicroserviceUrl))
        throw new Exception("LOGIN_MS_URL não encontrada!");

    client.BaseAddress = new Uri(loginMicroserviceUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddHttpClient<IOrderMsGateway, OrderMsGateway>(client =>
{
    string? orderMicroserviceUrl = builder.Configuration["ORDER_MS_URL"];

    if (string.IsNullOrEmpty(orderMicroserviceUrl))
        throw new Exception("ORDER_MS_URL não encontrada!");

    client.BaseAddress = new Uri(orderMicroserviceUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

builder.Services.AddHttpClient<IPaymentMsGateway, PaymentMsGateway>(client =>
{
    string? paymentMicroserviceUrl = builder.Configuration["PAYMENT_MS_URL"];

    if (string.IsNullOrEmpty(paymentMicroserviceUrl))
        throw new Exception("PAYMENT_MS_URL não encontrada!");

    client.BaseAddress = new Uri(paymentMicroserviceUrl);
    client.Timeout = TimeSpan.FromSeconds(60);
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
