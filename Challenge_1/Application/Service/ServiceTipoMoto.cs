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
    public class ServiceTipoMoto : InterfaceTipoMoto
    {
        private readonly AppDbContext _context;

        public ServiceTipoMoto(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoMotoDTO>> GetAllAsync()
        {
            var lista = await _context.TiposMoto.ToListAsync();
            return lista.Select(t => new TipoMotoDTO
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Categoria = t.Categoria
            });
        }

        public async Task<TipoMotoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.TiposMoto.FindAsync(id);
            if (entidade == null)
                return null;

            return new TipoMotoDTO
            {
                Id = entidade.Id,
                Descricao = entidade.Descricao,
                Categoria = entidade.Categoria
            };
        }

        public async Task<TipoMotoDTO> CreateAsync(TipoMotoDTO dto)
        {
            try
            {
                var entidade = new TipoMotoEntity
                {
                    Id = dto.Id,
                    Descricao = dto.Descricao,
                    Categoria = dto.Categoria
                };

                _context.TiposMoto.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar TipoMoto: " + ex.Message);
            }
        }

        public async Task<TipoMotoDTO> UpdateAsync(long id, TipoMotoDTO dto)
        {
            try
            {
                var entidade = await _context.TiposMoto.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Descricao = dto.Descricao;
                entidade.Categoria = dto.Categoria;

                _context.TiposMoto.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar TipoMoto: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.TiposMoto.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.TiposMoto.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir TipoMoto: " + ex.Message);
            }
        }

        public async Task<IEnumerable<TipoMotoDTO>> GetByDescricaoAsync(string descricao)
        {
            var lista = await _context.TiposMoto
                .Where(t => t.Descricao.ToLower().Contains(descricao.ToLower()))
                .ToListAsync();

            return lista.Select(t => new TipoMotoDTO
            {
                Id = t.Id,
                Descricao = t.Descricao,
                Categoria = t.Categoria
            });
        }
    }
}
