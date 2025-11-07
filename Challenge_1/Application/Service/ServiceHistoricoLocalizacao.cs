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
    public class ServiceHistoricoLocalizacao : InterfaceHistoricoLocalizacao
    {
        private readonly AppDbContext _context;

        public ServiceHistoricoLocalizacao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistoricoLocalizacaoDTO>> GetAllAsync()
        {
            var lista = await _context.HistoricosLocalizacao.ToListAsync();
            return lista.Select(h => new HistoricoLocalizacaoDTO
            {
                Id = h.Id,
                IdMoto = h.IdMoto,
                IdMotociclista = h.IdMotociclista,
                IdZonaPatio = h.IdZonaPatio,
                DataHoraSaida = h.DataHoraSaida,
                DataHoraEntrada = h.DataHoraEntrada,
                KmRodados = h.KmRodados,
                IdStatusOperacao = h.IdStatusOperacao
            });
        }

        public async Task<HistoricoLocalizacaoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.HistoricosLocalizacao.FindAsync(id);
            if (entidade == null)
                return null;

            return new HistoricoLocalizacaoDTO
            {
                Id = entidade.Id,
                IdMoto = entidade.IdMoto,
                IdMotociclista = entidade.IdMotociclista,
                IdZonaPatio = entidade.IdZonaPatio,
                DataHoraSaida = entidade.DataHoraSaida,
                DataHoraEntrada = entidade.DataHoraEntrada,
                KmRodados = entidade.KmRodados,
                IdStatusOperacao = entidade.IdStatusOperacao
            };
        }

        public async Task<HistoricoLocalizacaoDTO> CreateAsync(HistoricoLocalizacaoDTO dto)
        {
            try
            {
                var entidade = new HistoricoLocalizacaoEntity
                {
                    
                    IdMoto = dto.IdMoto,
                    IdMotociclista = dto.IdMotociclista,
                    IdZonaPatio = dto.IdZonaPatio,
                    DataHoraSaida = dto.DataHoraSaida,
                    DataHoraEntrada = dto.DataHoraEntrada,
                    KmRodados = dto.KmRodados,
                    IdStatusOperacao = dto.IdStatusOperacao
                };

                _context.HistoricosLocalizacao.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar histórico de localização: " + ex.Message);
            }
        }

        public async Task<HistoricoLocalizacaoDTO> UpdateAsync(long id, HistoricoLocalizacaoDTO dto)
        {
            try
            {
                var entidade = await _context.HistoricosLocalizacao.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.IdMoto = dto.IdMoto;
                entidade.IdMotociclista = dto.IdMotociclista;
                entidade.IdZonaPatio = dto.IdZonaPatio;
                entidade.DataHoraSaida = dto.DataHoraSaida;
                entidade.DataHoraEntrada = dto.DataHoraEntrada;
                entidade.KmRodados = dto.KmRodados;
                entidade.IdStatusOperacao = dto.IdStatusOperacao;

                _context.HistoricosLocalizacao.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar histórico de localização: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.HistoricosLocalizacao.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.HistoricosLocalizacao.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir histórico de localização: " + ex.Message);
            }
        }

        public async Task<IEnumerable<HistoricoLocalizacaoDTO>> GetByIdMotoAsync(long idMoto)
        {
            var lista = await _context.HistoricosLocalizacao
                .Where(h => h.IdMoto == idMoto)
                .ToListAsync();

            return lista.Select(h => new HistoricoLocalizacaoDTO
            {
                Id = h.Id,
                IdMoto = h.IdMoto,
                IdMotociclista = h.IdMotociclista,
                IdZonaPatio = h.IdZonaPatio,
                DataHoraSaida = h.DataHoraSaida,
                DataHoraEntrada = h.DataHoraEntrada,
                KmRodados = h.KmRodados,
                IdStatusOperacao = h.IdStatusOperacao
            });
        }
    }
}
