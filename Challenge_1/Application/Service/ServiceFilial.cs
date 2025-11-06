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
    public class ServiceFilial : InterfaceFilial
    {
        private readonly AppDbContext _context;

        public ServiceFilial(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FilialDTO>> GetAllAsync()
        {
            var lista = await _context.Filiais.ToListAsync();
            return lista.Select(f => new FilialDTO
            {
                Id = f.Id,
                Nome = f.Nome,
                Cnpj = f.Cnpj,
                Telefone = f.Telefone,
                Email = f.Email,
                Ativo = f.Ativo,
                IdEndereco = f.IdEndereco
            });
        }

        public async Task<FilialDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.Filiais.FindAsync(id);
            if (entidade == null)
                return null;

            return new FilialDTO
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                Cnpj = entidade.Cnpj,
                Telefone = entidade.Telefone,
                Email = entidade.Email,
                Ativo = entidade.Ativo,
                IdEndereco = entidade.IdEndereco
            };
        }

        public async Task<FilialDTO> CreateAsync(FilialDTO dto)
        {
            try
            {
                var entidade = new FilialEntity
                {
                    Id = dto.Id,
                    Nome = dto.Nome,
                    Cnpj = dto.Cnpj,
                    Telefone = dto.Telefone,
                    Email = dto.Email,
                    Ativo = dto.Ativo,
                    IdEndereco = dto.IdEndereco
                };

                _context.Filiais.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Filial: " + ex.Message);
            }
        }

        public async Task<FilialDTO> UpdateAsync(long id, FilialDTO dto)
        {
            try
            {
                var entidade = await _context.Filiais.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Nome = dto.Nome;
                entidade.Cnpj = dto.Cnpj;
                entidade.Telefone = dto.Telefone;
                entidade.Email = dto.Email;
                entidade.Ativo = dto.Ativo;
                entidade.IdEndereco = dto.IdEndereco;

                _context.Filiais.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Filial: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.Filiais.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.Filiais.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Filial: " + ex.Message);
            }
        }

        public async Task<IEnumerable<FilialDTO>> GetByNomeAsync(string nome)
        {
            var lista = await _context.Filiais
                .Where(f => f.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();

            return lista.Select(f => new FilialDTO
            {
                Id = f.Id,
                Nome = f.Nome,
                Cnpj = f.Cnpj,
                Telefone = f.Telefone,
                Email = f.Email,
                Ativo = f.Ativo,
                IdEndereco = f.IdEndereco
            });
        }
    }
}
