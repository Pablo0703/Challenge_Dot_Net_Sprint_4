using Application.Services;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Challenge.Tests.App.Service
{
    public class ZonaPatioServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar nova zona de pátio com sucesso")]
        public async Task CreateAsync_DeveCriarZonaPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_ZonaPatio_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceZonaPatio(context);

            var dto = new ZonaPatioDTO
            {
                IdPatio = 1,
                NomeZona = "Zona A",
                TipoZona = "Coberta",
                Capacidade = 20
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Zona A", result.NomeZona);
            Assert.True(context.ZonasPatio.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar zona existente")]
        public async Task GetByIdAsync_DeveRetornarZonaPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_ZonaPatio_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new ZonaPatioEntity
            {
                IdPatio = 1,
                NomeZona = "Zona B",
                TipoZona = "Aberta",
                Capacidade = 15
            };
            context.ZonasPatio.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceZonaPatio(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Zona B", result.NomeZona);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar zona existente")]
        public async Task UpdateAsync_DeveAtualizarZonaPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_ZonaPatio_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new ZonaPatioEntity
            {
                IdPatio = 1,
                NomeZona = "Zona C",
                TipoZona = "Coberta",
                Capacidade = 10
            };
            context.ZonasPatio.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceZonaPatio(context);
            var dto = new ZonaPatioDTO
            {
                Id = entity.Id,
                IdPatio = 2,
                NomeZona = "Zona C Atualizada",
                TipoZona = "Aberta",
                Capacidade = 25
            };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Zona C Atualizada", result.NomeZona);
        }

        [Fact(DisplayName = "DeleteAsync deve remover zona existente")]
        public async Task DeleteAsync_DeveRemoverZonaPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_ZonaPatio_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new ZonaPatioEntity
            {
                IdPatio = 1,
                NomeZona = "Zona D",
                TipoZona = "Coberta",
                Capacidade = 12
            };
            context.ZonasPatio.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceZonaPatio(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.ZonasPatio.Any());
        }

        [Fact(DisplayName = "GetByNomeZonaAsync deve retornar lista de zonas")]
        public async Task GetByNomeZonaAsync_DeveRetornarLista()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_ZonaPatio_{Guid.NewGuid()}")
                .Options;

            // Contexto 1 → adiciona dados
            using (var context = new AppDbContext(options))
            {
                context.ZonasPatio.AddRange(
                    new ZonaPatioEntity { Id = 9001, IdPatio = 1, NomeZona = "Zona Norte", TipoZona = "Aberta", Capacidade = 10 },
                    new ZonaPatioEntity { Id = 9002, IdPatio = 1, NomeZona = "Zona Norte Extensão", TipoZona = "Coberta", Capacidade = 20 }
                );
                await context.SaveChangesAsync();
            }

            // Contexto 2 → consulta
            using (var context = new AppDbContext(options))
            {
                var service = new ServiceZonaPatio(context);
                var result = await service.GetByNomeZonaAsync("Norte");

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.All(result, z => Assert.Contains("Norte", z.NomeZona));
            }
        }
    }
}