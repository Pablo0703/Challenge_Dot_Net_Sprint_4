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
    public class StatusOperacaoServiceTest
    {
        [Fact(DisplayName = "CreateAsync deve criar novo status de operação com sucesso")]
        public async Task CreateAsync_DeveCriarStatusOperacao()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var service = new ServiceStatusOperacao(context);

            var dto = new StatusOperacaoDTO
            {
                Descricao = "Em Andamento",
                TipoMovimentacao = "Entrada"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Em Andamento", result.Descricao);
            Assert.True(context.StatusOperacoes.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar status de operação existente")]
        public async Task GetByIdAsync_DeveRetornarStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusOperacaoEntity { Descricao = "Concluído", TipoMovimentacao = "Saída" };
            context.StatusOperacoes.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusOperacao(context);
            var result = await service.GetByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.Equal("Concluído", result.Descricao);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar status de operação existente")]
        public async Task UpdateAsync_DeveAtualizarStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusOperacaoEntity { Descricao = "Pendente", TipoMovimentacao = "Entrada" };
            context.StatusOperacoes.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusOperacao(context);
            var dto = new StatusOperacaoDTO
            {
                Id = entity.Id,
                Descricao = "Finalizado",
                TipoMovimentacao = "Saída"
            };

            var result = await service.UpdateAsync(entity.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Finalizado", result.Descricao);
        }

        [Fact(DisplayName = "DeleteAsync deve remover status de operação existente")]
        public async Task DeleteAsync_DeveRemoverStatus()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            var entity = new StatusOperacaoEntity { Descricao = "Cancelado", TipoMovimentacao = "Saída" };
            context.StatusOperacoes.Add(entity);
            await context.SaveChangesAsync();

            var service = new ServiceStatusOperacao(context);
            var result = await service.DeleteAsync(entity.Id);

            Assert.True(result);
            Assert.False(context.StatusOperacoes.Any());
        }

        [Fact(DisplayName = "GetByDescricaoAsync deve retornar lista de status")]
        public async Task GetByDescricaoAsync_DeveRetornarLista()
        {
            // ✅ Cria um banco único para o teste
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase($"DbTest_StatusOperacao_{Guid.NewGuid()}")
                .Options;

            // ✅ Primeiro contexto — insere os dados
            using (var context = new AppDbContext(options))
            {
                context.StatusOperacoes.AddRange(
                    new StatusOperacaoEntity
                    {
                        Id = 1001, // garante ID único
                        Descricao = "Operação Interna",
                        TipoMovimentacao = "Entrada"
                    },
                    new StatusOperacaoEntity
                    {
                        Id = 1002,
                        Descricao = "Operação Externa",
                        TipoMovimentacao = "Saída"
                    }
                );
                await context.SaveChangesAsync();
            }

            // ✅ Segundo contexto — consulta (sem tracking antigo)
            using (var context = new AppDbContext(options))
            {
                var service = new ServiceStatusOperacao(context);

                var result = await service.GetByDescricaoAsync("Operação");

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.All(result, s => Assert.Contains("Operação", s.Descricao));
            }
        }

    }
}