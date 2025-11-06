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
    public class ServiceStatusOperacao : InterfaceStatusOperacao
    {
        private readonly AppDbContext _context;

        public ServiceStatusOperacao(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StatusOperacaoDTO>> GetAllAsync()
        {
            var lista = await _context.StatusOperacoes.ToListAsync();
            return lista.Select(s => new StatusOperacaoDTO
            {
                Id = s.Id,
                Descricao = s.Descricao,
                TipoMovimentacao = s.TipoMovimentacao
            });
        }

        public async Task<StatusOperacaoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.StatusOperacoes.FindAsync(id);
            if (entidade == null)
                return null;

            return new StatusOperacaoDTO
            {
                Id = entidade.Id,
                Descricao = entidade.Descricao,
                TipoMovimentacao = entidade.TipoMovimentacao
            };
        }

        public async Task<StatusOperacaoDTO> CreateAsync(StatusOperacaoDTO dto)
        {
            try
            {
                var entidade = new StatusOperacaoEntity
                {
                    Id = dto.Id,
                    Descricao = dto.Descricao,
                    TipoMovimentacao = dto.TipoMovimentacao
                };

                _context.StatusOperacoes.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar StatusOperacao: " + ex.Message);
            }
        }

        public async Task<StatusOperacaoDTO> UpdateAsync(long id, StatusOperacaoDTO dto)
        {
            try
            {
                var entidade = await _context.StatusOperacoes.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Descricao = dto.Descricao;
                entidade.TipoMovimentacao = dto.TipoMovimentacao;

                _context.StatusOperacoes.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar StatusOperacao: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.StatusOperacoes.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.StatusOperacoes.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir StatusOperacao: " + ex.Message);
            }
        }

        public async Task<IEnumerable<StatusOperacaoDTO>> GetByDescricaoAsync(string descricao)
        {
            var lista = await _context.StatusOperacoes
                .Where(s => s.Descricao.ToLower().Contains(descricao.ToLower()))
                .ToListAsync();

            return lista.Select(s => new StatusOperacaoDTO
            {
                Id = s.Id,
                Descricao = s.Descricao,
                TipoMovimentacao = s.TipoMovimentacao
            });
        }
    }
}
