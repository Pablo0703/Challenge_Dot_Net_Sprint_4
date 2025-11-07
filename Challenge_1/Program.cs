using Infrastructure.IoC;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// ======================================
// 🔧 INJEÇÃO DE DEPENDÊNCIAS CENTRALIZADA
// ======================================
builder.Services.AddIoC(builder.Configuration);

// ======================================
// ⚙️ CONTROLLERS E SWAGGER
// ======================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Challenge API - Gestão de Pátio",
        Version = "v1",
        Description = "API RESTful para gestão de Motos, Pátios e Zonas de Pátio com boas práticas REST (CRUD, Paginação, HATEOAS).",
        Contact = new OpenApiContact
        {
            Name = "Grupo Challenge 2025",
            Email = "contato@mottu.com.br"
        }
    });

    c.EnableAnnotations();
    c.ExampleFilters();

    // Inclui XML (se existir)
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);

    // ======================================
    // 🔒 CONFIGURAÇÃO JWT NO SWAGGER
    // ======================================
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insira o token JWT desta forma: Bearer {seu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

// ======================================
// 🔐 CONFIGURAÇÃO DE AUTENTICAÇÃO JWT
// ======================================
var jwtKey = builder.Configuration["Jwt:Key"] ?? "ChaveSuperSecreta123456789";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// ======================================
// 🚦 RATE LIMITING
// ======================================
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("rateLimitePolicy", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromSeconds(20);
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        opt.QueueLimit = 2;
    });

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// ======================================
// 🗜️ COMPRESSÃO DE RESPOSTAS
// ======================================
builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});
builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Fastest;
});

// ======================================
// 🧩 VERSIONAMENTO DA API
// ======================================
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV"; // v1, v2...
    options.SubstituteApiVersionInUrl = true;
});

// ======================================
// 🚀 BUILD E MIDDLEWARES
// ======================================
var app = builder.Build();

// 🧾 SWAGGER
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Challenge API v1");
        c.RoutePrefix = string.Empty;
    });
}

// 🔒 MIDDLEWARES GERAIS
app.UseAuthentication(); 
app.UseAuthorization();
app.UseRateLimiter();
app.UseResponseCompression();

// 🌐 CONTROLLERS
app.MapControllers();

app.Run();

public partial class Program { }