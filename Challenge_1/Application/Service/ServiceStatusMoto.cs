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
    public class ServiceStatusMoto : InterfaceStatusMoto
    {
        private readonly AppDbContext _context;

        public ServiceStatusMoto(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StatusMotoDTO>> GetAllAsync()
        {
            var lista = await _context.StatusMotos.ToListAsync();
            return lista.Select(s => new StatusMotoDTO
            {
                Id = s.Id,
                Descricao = s.Descricao,
                Disponivel = s.Disponivel
            });
        }

        public async Task<StatusMotoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.StatusMotos.FindAsync(id);
            if (entidade == null)
                return null;

            return new StatusMotoDTO
            {
                Id = entidade.Id,
                Descricao = entidade.Descricao,
                Disponivel = entidade.Disponivel
            };
        }

        public async Task<StatusMotoDTO> CreateAsync(StatusMotoDTO dto)
        {
            try
            {
                var entidade = new StatusMotoEntity
                {
                    Id = dto.Id,
                    Descricao = dto.Descricao,
                    Disponivel = dto.Disponivel
                };

                _context.StatusMotos.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar StatusMoto: " + ex.Message);
            }
        }

        public async Task<StatusMotoDTO> UpdateAsync(long id, StatusMotoDTO dto)
        {
            try
            {
                var entidade = await _context.StatusMotos.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Descricao = dto.Descricao;
                entidade.Disponivel = dto.Disponivel;

                _context.StatusMotos.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar StatusMoto: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.StatusMotos.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.StatusMotos.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir StatusMoto: " + ex.Message);
            }
        }

        public async Task<IEnumerable<StatusMotoDTO>> GetByNomeAsync(string nome)
        {
            var lista = await _context.StatusMotos
                .Where(s => s.Descricao.ToLower().Contains(nome.ToLower()))
                .ToListAsync();

            return lista.Select(s => new StatusMotoDTO
            {
                Id = s.Id,
                Descricao = s.Descricao,
                Disponivel = s.Disponivel
            });
        }
    }
}
