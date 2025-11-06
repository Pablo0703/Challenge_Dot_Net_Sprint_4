using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthCheck
{
    public class BancoHealthChecks : IHealthCheck
    {
        private readonly AppDbContext _context;

        public BancoHealthChecks(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // 🔹 Faz uma consulta simples ao Oracle (ping)
                await _context.Database.ExecuteSqlRawAsync("SELECT 1 FROM DUAL", cancellationToken);

                return HealthCheckResult.Healthy("Oracle respondeu à consulta.");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Falha ao consultar o banco Oracle.", ex);
            }
        }
    }
}
