using Application.Interfaces;
using Application.Services;
using Challenge_1.Application.Service.Auth;
using Infrastructure.Data;
using Infrastructure.HealthCheck;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static void AddIoC(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔹 CONTEXTO DO BANCO ORACLE
            services.AddDbContext<AppDbContext>(options =>
                options.UseOracle(configuration.GetConnectionString("OracleConnection"))
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors());

            // 🔹 HEALTH CHECKS (LIVENESS + READINESS)
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "live" })
                .AddCheck<BancoHealthChecks>("oracle_ef_query", tags: new[] { "ready" });

            // 🔹 INJEÇÃO DE DEPENDÊNCIAS - SERVICES
            services.AddScoped<InterfaceEndereco, ServiceEndereco>();
            services.AddScoped<InterfaceNotaFiscal, ServiceNotaFiscal>();
            services.AddScoped<InterfaceStatusMoto, ServiceStatusMoto>();
            services.AddScoped<InterfaceStatusOperacao, ServiceStatusOperacao>();
            services.AddScoped<InterfaceTipoMoto, ServiceTipoMoto>();
            services.AddScoped<InterfaceFilial, ServiceFilial>();
            services.AddScoped<InterfacePatio, ServicePatio>();
            services.AddScoped<InterfaceZonaPatio, ServiceZonaPatio>();
            services.AddScoped<InterfaceMoto, ServiceMoto>();
            services.AddScoped<InterfaceMotociclista, ServiceMotociclista>();
            services.AddScoped<InterfaceLocalizacaoMoto, ServiceLocalizacaoMoto>();
            services.AddScoped<InterfaceHistoricoLocalizacao, ServiceHistoricoLocalizacao>();
            services.AddScoped<JwtService>();
        }
    }
}
