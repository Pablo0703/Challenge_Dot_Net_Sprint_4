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
    public class MotociclistaServiceTest
    {
        private DbContextOptions<AppDbContext> GetOptions() =>
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

        // 🔧 Helper para criar entidades completas (evita erro de propriedades obrigatórias)
        private MotociclistaEntity CriarMotociclistaPadrao(string nome = "Teste") =>
            new MotociclistaEntity
            {
                Nome = nome,
                Cpf = "12345678900",
                Cnh = "CNH12345",
                Telefone = "(11) 99999-9999",
                Email = $"{nome.ToLower()}@teste.com",
                DataCadastro = DateTime.Now,
                Ativo = "S"
            };

        [Fact(DisplayName = "CreateAsync deve criar motociclista com sucesso")]
        public async Task CreateAsync_DeveCriarMotociclista()
        {
            var options = GetOptions();
            using var context = new AppDbContext(options);
            var service = new ServiceMotociclista(context);

            var dto = new MotociclistaDTO
            {
                Nome = "Lucas",
                Cpf = "12345678900",
                Cnh = "CNH12345",
                Telefone = "(11) 98888-7777",
                Email = "lucas@teste.com",
                DataCadastro = DateTime.Now,
                Ativo = "S"
            };

            var result = await service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Lucas", result.Nome);
            Assert.True(context.Motociclistas.Any());
        }

        [Fact(DisplayName = "GetByIdAsync deve retornar motociclista existente")]
        public async Task GetByIdAsync_DeveRetornarMotociclista()
        {
            var options = GetOptions();
            using var context = new AppDbContext(options);
            var entidade = CriarMotociclistaPadrao("Paulo");
            context.Motociclistas.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceMotociclista(context);
            var result = await service.GetByIdAsync(entidade.Id);

            Assert.NotNull(result);
            Assert.Equal("Paulo", result.Nome);
        }

        [Fact(DisplayName = "UpdateAsync deve atualizar motociclista existente")]
        public async Task UpdateAsync_DeveAtualizarMotociclista()
        {
            var options = GetOptions();
            using var context = new AppDbContext(options);
            var entidade = CriarMotociclistaPadrao("Rafael");
            context.Motociclistas.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceMotociclista(context);
            var dto = new MotociclistaDTO
            {
                Id = entidade.Id,
                Nome = "Rafael Atualizado",
                Cpf = entidade.Cpf,
                Cnh = entidade.Cnh,
                Telefone = entidade.Telefone,
                Email = entidade.Email,
                DataCadastro = entidade.DataCadastro,
                Ativo = "S"
            };

            var result = await service.UpdateAsync(entidade.Id, dto);

            Assert.NotNull(result);
            Assert.Equal("Rafael Atualizado", result.Nome);
        }

        [Fact(DisplayName = "DeleteAsync deve remover motociclista existente")]
        public async Task DeleteAsync_DeveRemoverMotociclista()
        {
            var options = GetOptions();
            using var context = new AppDbContext(options);
            var entidade = CriarMotociclistaPadrao("Thiago");
            context.Motociclistas.Add(entidade);
            await context.SaveChangesAsync();

            var service = new ServiceMotociclista(context);
            var result = await service.DeleteAsync(entidade.Id);

            Assert.True(result);
            Assert.False(context.Motociclistas.Any());
        }

        [Fact(DisplayName = "GetByNomeAsync deve retornar lista de motociclistas")]
        public async Task GetByNomeAsync_DeveRetornarLista()
        {
            var options = GetOptions();
            using var context = new AppDbContext(options);
            var service = new ServiceMotociclista(context);

            context.Motociclistas.AddRange(
                new MotociclistaEntity
                {
                    Id = 1,
                    Nome = "André",
                    Cpf = "12345678901",
                    Cnh = "CNH001",
                    Telefone = "(11) 90000-0001",
                    Email = "andre@teste.com",
                    DataCadastro = DateTime.Now,
                    Ativo = "S"
                },
                new MotociclistaEntity
                {
                    Id = 2,
                    Nome = "Anderson",
                    Cpf = "12345678902",
                    Cnh = "CNH002",
                    Telefone = "(11) 90000-0002",
                    Email = "anderson@teste.com",
                    DataCadastro = DateTime.Now,
                    Ativo = "S"
                 }
                );

            await context.SaveChangesAsync();

            var result = await service.GetByNomeAsync("And");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.Contains("And", r.Nome));
        }
    }
}
