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
    public class PatioServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar um novo pátio com sucesso")]
        public async Task CreateAsync_DeveCriarPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ✅ banco isolado
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServicePatio(context);

            var dto = new PatioDTO
            {
                IdFilial = 1,
                Nome = "Pátio Central",
                AreaM2 = 500,
                Capacidade = 50
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Pátio Central", result.Nome);
            Assert.True(context.Patios.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar um pátio existente")]
        public async Task GetByIdAsync_DeveRetornarPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new PatioEntity { IdFilial = 1, Nome = "Pátio Norte", AreaM2 = 300, Capacidade = 30 };
            context.Patios.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServicePatio(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Pátio Norte", result.Nome);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar um pátio existente")]
        public async Task UpdateAsync_DeveAtualizarPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new PatioEntity { IdFilial = 1, Nome = "Pátio Leste", AreaM2 = 400, Capacidade = 40 };
            context.Patios.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServicePatio(context);
            var dto = new PatioDTO { Id = entity.Id, IdFilial = 2, Nome = "Pátio Leste Atualizado", AreaM2 = 450, Capacidade = 60 };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Pátio Leste Atualizado", result.Nome);
        }

        [Fact(DisplayName = "DeleteAsync deve remover um pátio existente")]
        public async Task DeleteAsync_DeveRemoverPatio()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new PatioEntity { IdFilial = 1, Nome = "Pátio Oeste", AreaM2 = 200, Capacidade = 25 };
            context.Patios.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServicePatio(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.Patios.Any());
        }

        [Fact(DisplayName = "GetByNomeAsync deve retornar lista de pátios")]
        public async Task GetByNomeAsync_DeveRetornarLista()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco único por teste
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServicePatio(context);

            //  Adiciona entidades com IDs únicos para evitar conflito de rastreamento
            context.Patios.AddRange(
                new PatioEntity
                {
                    Id = 1,
                    Nome = "Pátio Central",
                    AreaM2 = 200,
                    Capacidade = 100,
                    Ativo = "S",
                    IdFilial = 1
                },
                new PatioEntity
                {
                    Id = 2,
                    Nome = "Pátio Central Expandido",
                    AreaM2 = 300,
                    Capacidade = 150,
                    Ativo = "S",
                    IdFilial = 1
                }
            );

            await context.SaveChangesAsync();

            context.ChangeTracker.Clear();

            // Act
            var result = await service.GetByNomeAsync("Central");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, p => Assert.Contains("Central", p.Nome));
        }


    }
}