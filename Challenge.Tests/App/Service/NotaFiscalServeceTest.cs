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
    public class NotaFiscalServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar nota fiscal com sucesso")]
        public async Task CreateAsync_DeveCriarNotaFiscal()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceNotaFiscal(context);

            var dto = new NotaFiscalDTO
            {
                Numero = "NF0001",
                Serie = "1",
                Modelo = "55",
                ChaveAcesso = "12345678901234567890123456789012345678901234",
                DataEmissao = DateTime.UtcNow,
                ValorTotal = 1000M,
                Fornecedor = "Fornecedor A",
                CnpjFornecedor = "12345678000100"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("NF0001", result.Numero);
            Assert.True(context.NotasFiscais.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar nota fiscal existente")]
        public async Task GetByIdAsync_DeveRetornarNota()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new NotaFiscalEntity
            {
                Numero = "NF100",
                Serie = "A",
                Modelo = "55",
                ChaveAcesso = "98765432101234567890987654321098765432109876",
                DataEmissao = DateTime.UtcNow,
                ValorTotal = 500M,
                Fornecedor = "Fornecedor B",
                CnpjFornecedor = "12345678000199"
            };
            context.NotasFiscais.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceNotaFiscal(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("NF100", result.Numero);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar nota fiscal existente")]
        public async Task UpdateAsync_DeveAtualizarNota()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new NotaFiscalEntity
            {
                Numero = "NF200",
                Serie = "B",
                Modelo = "65",
                ChaveAcesso = "99999999999999999999999999999999999999999999",
                DataEmissao = DateTime.UtcNow,
                ValorTotal = 1200M,
                Fornecedor = "Fornecedor Antigo",
                CnpjFornecedor = "12345678000177"
            };
            context.NotasFiscais.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceNotaFiscal(context);
            var dto = new NotaFiscalDTO
            {
                Id = entity.Id,
                Numero = "NF200-EDIT",
                Serie = "B",
                Modelo = "65",
                ChaveAcesso = entity.ChaveAcesso,
                DataEmissao = entity.DataEmissao,
                ValorTotal = 1500M,
                Fornecedor = "Fornecedor Atualizado",
                CnpjFornecedor = entity.CnpjFornecedor
            };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("NF200-EDIT", result.Numero);
        }

        [Fact(DisplayName = "DeleteAsync deve remover nota fiscal existente")]
        public async Task DeleteAsync_DeveRemoverNota()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new NotaFiscalEntity
            {
                Numero = "NF300",
                Serie = "C",
                Modelo = "55",
                ChaveAcesso = "11111111111111111111111111111111111111111111",
                DataEmissao = DateTime.UtcNow,
                ValorTotal = 2000M,
                Fornecedor = "Fornecedor Z",
                CnpjFornecedor = "98765432000100"
            };
            context.NotasFiscais.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceNotaFiscal(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.NotasFiscais.Any());
        }

        [Fact(DisplayName = "GetByNumeroAsync deve retornar lista de notas")]
        public async Task GetByNumeroAsync_DeveRetornarLista()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // banco isolado
                .Options;

            using var context = new AppDbContext(options);

            //IDs únicos para evitar conflitos de rastreamento
            context.NotasFiscais.AddRange(
                new NotaFiscalEntity
                {
                    Id = 1001,
                    Numero = "NF123",
                    Serie = "A1",
                    Modelo = "55",
                    ChaveAcesso = "11111111111111111111111111111111111111111111",
                    DataEmissao = DateTime.UtcNow,
                    ValorTotal = 100M,
                    Fornecedor = "F1",
                    CnpjFornecedor = "0001"
                },
                new NotaFiscalEntity
                {
                    Id = 1002,
                    Numero = "NF124",
                    Serie = "A2",
                    Modelo = "55",
                    ChaveAcesso = "22222222222222222222222222222222222222222222",
                    DataEmissao = DateTime.UtcNow,
                    ValorTotal = 200M,
                    Fornecedor = "F2",
                    CnpjFornecedor = "0002"
                }
            );

            await context.SaveChangesAsync();

            var service = new ServiceNotaFiscal(context);
            var result = await service.GetByNumeroAsync("NF1");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

    }
}
