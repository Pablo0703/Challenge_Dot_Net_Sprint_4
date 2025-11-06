using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Presentation.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServicePatio : InterfacePatio
    {
        private readonly AppDbContext _context;

        public ServicePatio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PatioDTO>> GetAllAsync()
        {
            var lista = await _context.Patios.ToListAsync();
            return lista.Select(p => new PatioDTO
            {
                Id = p.Id,
                IdFilial = p.IdFilial,
                Nome = p.Nome,
                AreaM2 = p.AreaM2,
                Capacidade = p.Capacidade,
                Ativo = p.Ativo
            });
        }

        public async Task<PatioDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.Patios.FindAsync(id);
            if (entidade == null)
                return null;

            return new PatioDTO
            {
                Id = entidade.Id,
                IdFilial = entidade.IdFilial,
                Nome = entidade.Nome,
                AreaM2 = entidade.AreaM2,
                Capacidade = entidade.Capacidade,
                Ativo = entidade.Ativo
            };
        }

        public async Task<PatioDTO> CreateAsync(PatioDTO dto)
        {
            try
            {
                // gera próximo ID manualmente (Max + 1)
                var nextId = (await _context.Patios.MaxAsync(p => (long?)p.Id) ?? 0) + 1;

                var entidade = new PatioEntity
                {
                    Id = nextId, 
                    IdFilial = dto.IdFilial,
                    Nome = dto.Nome,
                    AreaM2 = dto.AreaM2,
                    Capacidade = dto.Capacidade,
                    Ativo = dto.Ativo ?? "S" // se não mandar, assume ativo
                };

                _context.Patios.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Patio: " + ex.Message, ex);
            }
        }


        public async Task<PatioDTO> UpdateAsync(long id, PatioDTO dto)
        {
            try
            {
                var entidade = await _context.Patios.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.IdFilial = dto.IdFilial;
                entidade.Nome = dto.Nome;
                entidade.AreaM2 = dto.AreaM2;
                entidade.Capacidade = dto.Capacidade;
                entidade.Ativo = dto.Ativo;

                _context.Patios.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Patio: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.Patios.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.Patios.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Patio: " + ex.Message);
            }
        }

        public async Task<IEnumerable<PatioDTO>> GetByNomeAsync(string nome)
        {
            var lista = await _context.Patios
                .Where(p => p.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();

            return lista.Select(p => new PatioDTO
            {
                Id = p.Id,
                IdFilial = p.IdFilial,
                Nome = p.Nome,
                AreaM2 = p.AreaM2,
                Capacidade = p.Capacidade,
                Ativo = p.Ativo
            });
        }
    }
}
