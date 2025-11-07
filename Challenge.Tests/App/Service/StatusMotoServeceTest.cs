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
    public class StatusMotoServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar um novo status de moto com sucesso")]
        public async Task CreateAsync_DeveCriarStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceStatusMoto(context);

            var dto = new StatusMotoDTO
            {
                Descricao = "Disponível",
                Disponivel = "S"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Disponível", result.Descricao);
            Assert.True(context.StatusMotos.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar status existente")]
        public async Task GetByIdAsync_DeveRetornarStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusMotoEntity { Descricao = "Em Manutenção", Disponivel = "N" };
            context.StatusMotos.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusMoto(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Em Manutenção", result.Descricao);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar status existente")]
        public async Task UpdateAsync_DeveAtualizarStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusMotoEntity { Descricao = "Indisponível", Disponivel = "N" };
            context.StatusMotos.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusMoto(context);
            var dto = new StatusMotoDTO { Id = entity.Id, Descricao = "Disponível", Disponivel = "S" };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Disponível", result.Descricao);
        }

        [Fact(DisplayName = "DeleteAsync deve remover status existente")]
        public async Task DeleteAsync_DeveRemoverStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusMotoEntity { Descricao = "Aposentada", Disponivel = "N" };
            context.StatusMotos.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusMoto(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.StatusMotos.Any());
        }

        [Fact(DisplayName = "GetByNomeAsync deve retornar lista de status")]
        public async Task GetByNomeAsync_DeveRetornarLista()
        {
            // 🔹 Banco completamente isolado (nome único)
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // 🔹 Cria novo contexto EF (não reaproveita o anterior)
            using var context = new AppDbContext(options);
            var service = new ServiceStatusMoto(context);

            // 🔹 Define IDs manuais diferentes (para evitar conflito de rastreamento)
            context.StatusMotos.AddRange(
                new StatusMotoEntity
                {
                    Id = 1001, // 🔸 Ids distintos
                    Descricao = "Disponível para uso",
                    Disponivel = "S"
                },
                new StatusMotoEntity
                {
                    Id = 1002, // 🔸 Ids distintos
                    Descricao = "Disponível para manutenção",
                    Disponivel = "S"
                }
            );

            await context.SaveChangesAsync();

            // Act
            var result = await service.GetByNomeAsync("Disponível");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, s => Assert.Contains("Disponível", s.Descricao));
        }
    }
}