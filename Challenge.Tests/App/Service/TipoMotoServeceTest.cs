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
    public class TipoMotoServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar novo tipo de moto com sucesso")]
        public async Task CreateAsync_DeveCriarTipoMoto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_TipoMoto_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceTipoMoto(context);

            var dto = new TipoMotoDTO
            {
                Descricao = "Street 150cc",
                Categoria = "Urbana"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Street 150cc", result.Descricao);
            Assert.True(context.TiposMoto.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar tipo de moto existente")]
        public async Task GetByIdAsync_DeveRetornarTipoMoto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_TipoMoto_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new TipoMotoEntity { Descricao = "Trail 300cc", Categoria = "Off-road" };
            context.TiposMoto.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceTipoMoto(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Trail 300cc", result.Descricao);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar tipo de moto existente")]
        public async Task UpdateAsync_DeveAtualizarTipoMoto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_TipoMoto_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new TipoMotoEntity { Descricao = "Scooter 125cc", Categoria = "Urbana" };
            context.TiposMoto.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceTipoMoto(context);
            var dto = new TipoMotoDTO
            {
                Id = entity.Id,
                Descricao = "Scooter 150cc",
                Categoria = "Urbana Plus"
            };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Scooter 150cc", result.Descricao);
        }

        [Fact(DisplayName = "DeleteAsync deve remover tipo de moto existente")]
        public async Task DeleteAsync_DeveRemoverTipoMoto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_TipoMoto_{Guid.NewGuid()}")
                .Options;

            using var context = new AppDbContext(options);
            var entity = new TipoMotoEntity { Descricao = "Custom 800cc", Categoria = "Premium" };
            context.TiposMoto.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceTipoMoto(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.TiposMoto.Any());
        }

        [Fact(DisplayName = "GetByDescricaoAsync deve retornar lista de tipos de moto")]
        public async Task GetByDescricaoAsync_DeveRetornarLista()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"Db_TipoMoto_{Guid.NewGuid()}")
                .Options;

            // Contexto 1 → insere
            using (var context = new AppDbContext(options))
            {
                context.TiposMoto.AddRange(
                    new TipoMotoEntity { Id = 2001, Descricao = "Street 150cc", Categoria = "Urbana" },
                    new TipoMotoEntity { Id = 2002, Descricao = "Street 250cc", Categoria = "Urbana" }
                );
                await context.SaveChangesAsync();
            }

            // Contexto 2 → consulta
            using (var context = new AppDbContext(options))
            {
                var service = new ServiceTipoMoto(context);
                var result = await service.GetByDescricaoAsync("Street");

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.All(result, r => Assert.Contains("Street", r.Descricao));
            }
        }
    }
}