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
    public class ServiceZonaPatio : InterfaceZonaPatio
    {
        private readonly AppDbContext _context;

        public ServiceZonaPatio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ZonaPatioDTO>> GetAllAsync()
        {
            var lista = await _context.ZonasPatio.ToListAsync();
            return lista.Select(z => new ZonaPatioDTO
            {
                Id = z.Id,
                IdPatio = z.IdPatio,
                NomeZona = z.NomeZona,
                TipoZona = z.TipoZona,
                Capacidade = z.Capacidade
            });
        }

        public async Task<ZonaPatioDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.ZonasPatio.FindAsync(id);
            if (entidade == null)
                return null;

            return new ZonaPatioDTO
            {
                Id = entidade.Id,
                IdPatio = entidade.IdPatio,
                NomeZona = entidade.NomeZona,
                TipoZona = entidade.TipoZona,
                Capacidade = entidade.Capacidade
            };
        }

        public async Task<ZonaPatioDTO> CreateAsync(ZonaPatioDTO dto)
        {
            try
            {
                var nextId = (await _context.ZonasPatio.MaxAsync(z => (long?)z.Id) ?? 0) + 1;

                var entidade = new ZonaPatioEntity
                {
                    Id = nextId,
                    IdPatio = dto.IdPatio,
                    NomeZona = dto.NomeZona,
                    TipoZona = dto.TipoZona,
                    Capacidade = dto.Capacidade
                };

                _context.ZonasPatio.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar ZonaPatio: " + ex.Message);
            }
        }


        public async Task<ZonaPatioDTO> UpdateAsync(long id, ZonaPatioDTO dto)
        {
            try
            {
                var entidade = await _context.ZonasPatio.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.IdPatio = dto.IdPatio;
                entidade.NomeZona = dto.NomeZona;
                entidade.TipoZona = dto.TipoZona;
                entidade.Capacidade = dto.Capacidade;

                _context.ZonasPatio.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar ZonaPatio: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.ZonasPatio.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.ZonasPatio.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir ZonaPatio: " + ex.Message);
            }
        }

        public async Task<IEnumerable<ZonaPatioDTO>> GetByNomeZonaAsync(string nomeZona)
        {
            var lista = await _context.ZonasPatio
                .Where(z => z.NomeZona.ToLower().Contains(nomeZona.ToLower()))
                .ToListAsync();

            return lista.Select(z => new ZonaPatioDTO
            {
                Id = z.Id,
                IdPatio = z.IdPatio,
                NomeZona = z.NomeZona,
                TipoZona = z.TipoZona,
                Capacidade = z.Capacidade
            });
        }
    }
}
