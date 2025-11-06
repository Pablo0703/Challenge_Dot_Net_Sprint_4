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
    public class ServiceLocalizacaoMoto : InterfaceLocalizacaoMoto
    {
        private readonly AppDbContext _context;

        public ServiceLocalizacaoMoto(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LocalizacaoMotoDTO>> GetAllAsync()
        {
            var lista = await _context.LocalizacoesMoto.ToListAsync();
            return lista.Select(l => new LocalizacaoMotoDTO
            {
                Id = l.Id,
                IdMoto = l.IdMoto,
                IdZona = l.IdZona,
                DataHoraEntrada = l.DataHoraEntrada,
                DataHoraSaida = l.DataHoraSaida
            });
        }

        public async Task<LocalizacaoMotoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.LocalizacoesMoto.FindAsync(id);
            if (entidade == null)
                return null;

            return new LocalizacaoMotoDTO
            {
                Id = entidade.Id,
                IdMoto = entidade.IdMoto,
                IdZona = entidade.IdZona,
                DataHoraEntrada = entidade.DataHoraEntrada,
                DataHoraSaida = entidade.DataHoraSaida
            };
        }

        public async Task<LocalizacaoMotoDTO> CreateAsync(LocalizacaoMotoDTO dto)
        {
            try
            {
                var entidade = new LocalizacaoMotoEntity
                {
                    Id = dto.Id,
                    IdMoto = dto.IdMoto,
                    IdZona = dto.IdZona,
                    DataHoraEntrada = dto.DataHoraEntrada,
                    DataHoraSaida = dto.DataHoraSaida
                };

                _context.LocalizacoesMoto.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Localização de Moto: " + ex.Message);
            }
        }

        public async Task<LocalizacaoMotoDTO> UpdateAsync(long id, LocalizacaoMotoDTO dto)
        {
            try
            {
                var entidade = await _context.LocalizacoesMoto.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.IdMoto = dto.IdMoto;
                entidade.IdZona = dto.IdZona;
                entidade.DataHoraEntrada = dto.DataHoraEntrada;
                entidade.DataHoraSaida = dto.DataHoraSaida;

                _context.LocalizacoesMoto.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Localização de Moto: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.LocalizacoesMoto.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.LocalizacoesMoto.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Localização de Moto: " + ex.Message);
            }
        }

        public async Task<IEnumerable<LocalizacaoMotoDTO>> GetByIdMotoAsync(long idMoto)
        {
            var lista = await _context.LocalizacoesMoto
                .Where(l => l.IdMoto == idMoto)
                .ToListAsync();

            return lista.Select(l => new LocalizacaoMotoDTO
            {
                Id = l.Id,
                IdMoto = l.IdMoto,
                IdZona = l.IdZona,
                DataHoraEntrada = l.DataHoraEntrada,
                DataHoraSaida = l.DataHoraSaida
            });
        }
    }
}
