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
    public class ServiceMoto : InterfaceMoto
    {
        private readonly AppDbContext _context;

        public ServiceMoto(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MotoDTO>> GetAllAsync()
        {
            var lista = await _context.Motos.ToListAsync();
            return lista.Select(m => new MotoDTO
            {
                Id = m.Id,
                IdTipo = m.IdTipo,
                IdStatus = m.IdStatus,
                Placa = m.Placa,
                Modelo = m.Modelo,
                AnoFabricacao = m.AnoFabricacao,
                AnoModelo = m.AnoModelo,
                Chassi = m.Chassi,
                Cilindrada = m.Cilindrada,
                Cor = m.Cor,
                DataAquisicao = m.DataAquisicao,
                ValorAquisicao = m.ValorAquisicao,
                IdNotaFiscal = m.IdNotaFiscal
            });
        }

        public async Task<MotoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.Motos.FindAsync(id);
            if (entidade == null)
                return null;

            return new MotoDTO
            {
                Id = entidade.Id,
                IdTipo = entidade.IdTipo,
                IdStatus = entidade.IdStatus,
                Placa = entidade.Placa,
                Modelo = entidade.Modelo,
                AnoFabricacao = entidade.AnoFabricacao,
                AnoModelo = entidade.AnoModelo,
                Chassi = entidade.Chassi,
                Cilindrada = entidade.Cilindrada,
                Cor = entidade.Cor,
                DataAquisicao = entidade.DataAquisicao,
                ValorAquisicao = entidade.ValorAquisicao,
                IdNotaFiscal = entidade.IdNotaFiscal
            };
        }

        public async Task<MotoDTO> CreateAsync(MotoDTO dto)
        {
            try
            {
                // Descobre o próximo ID
                var nextId = (await _context.Motos.MaxAsync(m => (long?)m.Id) ?? 0) + 1;

                var entidade = new MotoEntity
                {
                    Id = nextId, 
                    IdTipo = dto.IdTipo,
                    IdStatus = dto.IdStatus,
                    Placa = dto.Placa,
                    Modelo = dto.Modelo,
                    AnoFabricacao = dto.AnoFabricacao,
                    AnoModelo = dto.AnoModelo,
                    Chassi = dto.Chassi,
                    Cilindrada = dto.Cilindrada,
                    Cor = dto.Cor,
                    DataAquisicao = dto.DataAquisicao,
                    ValorAquisicao = dto.ValorAquisicao,
                    IdNotaFiscal = dto.IdNotaFiscal
                };

                _context.Motos.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Moto", ex);
            }
        }


        public async Task<MotoDTO> UpdateAsync(long id, MotoDTO dto)
        {
            try
            {
                var entidade = await _context.Motos.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.IdTipo = dto.IdTipo;
                entidade.IdStatus = dto.IdStatus;
                entidade.Placa = dto.Placa;
                entidade.Modelo = dto.Modelo;
                entidade.AnoFabricacao = dto.AnoFabricacao;
                entidade.AnoModelo = dto.AnoModelo;
                entidade.Chassi = dto.Chassi;
                entidade.Cilindrada = dto.Cilindrada;
                entidade.Cor = dto.Cor;
                entidade.DataAquisicao = dto.DataAquisicao;
                entidade.ValorAquisicao = dto.ValorAquisicao;
                entidade.IdNotaFiscal = dto.IdNotaFiscal;

                _context.Motos.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Moto: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.Motos.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.Motos.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Moto: " + ex.Message);
            }
        }

        public async Task<IEnumerable<MotoDTO>> GetByPlacaAsync(string placa)
        {
            var lista = await _context.Motos
                .Where(m => m.Placa.ToLower().Contains(placa.ToLower()))
                .ToListAsync();

            return lista.Select(m => new MotoDTO
            {
                Id = m.Id,
                IdTipo = m.IdTipo,
                IdStatus = m.IdStatus,
                Placa = m.Placa,
                Modelo = m.Modelo,
                AnoFabricacao = m.AnoFabricacao,
                AnoModelo = m.AnoModelo,
                Chassi = m.Chassi,
                Cilindrada = m.Cilindrada,
                Cor = m.Cor,
                DataAquisicao = m.DataAquisicao,
                ValorAquisicao = m.ValorAquisicao,
                IdNotaFiscal = m.IdNotaFiscal
            });
        }

    }
}
