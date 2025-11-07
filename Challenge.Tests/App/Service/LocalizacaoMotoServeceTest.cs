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
    public class LocalizacaoMotoServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar nova localização com sucesso")]
        public async Task CreateAsync_DeveCriarLocalizacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ✅ banco único
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceLocalizacaoMoto(context);

            var dto = new LocalizacaoMotoDTO
            {
                IdMoto = 1,
                IdZona = 5,
                DataHoraEntrada = DateTime.Now
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(1, result.IdMoto);
            Assert.Equal(5, result.IdZona);
            Assert.True(context.LocalizacoesMoto.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar localização existente")]
        public async Task GetByIdAsync_DeveRetornarLocalizacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // ✅ banco único
                .Options;

            using var context = new AppDbContext(options);
            var entidade = new LocalizacaoMotoEntity { IdMoto = 10, IdZona = 3 };
            context.LocalizacoesMoto.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceLocalizacaoMoto(context);
            var result = await service.GetByIdAsync(entidade.Id);

            Assert.NotNull(result);
            Assert.Equal(10, result.IdMoto);
            Assert.Equal(3, result.IdZona);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar localização existente")]
        public async Task UpdateAsync_DeveAtualizarLocalizacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entidade = new LocalizacaoMotoEntity { IdMoto = 2, IdZona = 4 };
            context.LocalizacoesMoto.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceLocalizacaoMoto(context);

            var dto = new LocalizacaoMotoDTO
            {
                Id = entidade.Id,
                IdMoto = 99,
                IdZona = 8
            };

            var result = await service.UpdateAsync(entidade.Id, dto);

            Assert.NotNull(result);
            Assert.Equal(99, result.IdMoto);
            Assert.Equal(8, result.IdZona);
        }

        [Fact(DisplayName = "DeleteAsync deve remover localização existente")]
        public async Task DeleteAsync_DeveRemoverLocalizacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entidade = new LocalizacaoMotoEntity { IdMoto = 3, IdZona = 1 };
            context.LocalizacoesMoto.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceLocalizacaoMoto(context);
            var result = await service.DeleteAsync(entidade.Id);

            Assert.True(result);
            Assert.False(context.LocalizacoesMoto.Any());
        }

        [Fact(DisplayName = "GetByIdMotoAsync deve retornar lista de localizações")]
        public async Task GetByIdMotoAsync_DeveRetornarLista()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceLocalizacaoMoto(context);

            context.LocalizacoesMoto.AddRange(
                new LocalizacaoMotoEntity { Id = 1001, IdMoto = 99, IdZona = 5 },
                new LocalizacaoMotoEntity { Id = 1002, IdMoto = 99, IdZona = 6 }
            );

            await context.SaveChangesAsync();

            var result = await service.GetByIdMotoAsync(99);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, l => Assert.Equal(99, l.IdMoto));
        }
    }
}