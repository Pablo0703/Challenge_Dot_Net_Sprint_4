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
    public class ServiceMotociclista : InterfaceMotociclista
    {
        private readonly AppDbContext _context;

        public ServiceMotociclista(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MotociclistaDTO>> GetAllAsync()
        {
            var lista = await _context.Motociclistas.ToListAsync();
            return lista.Select(m => new MotociclistaDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                Cpf = m.Cpf,
                Cnh = m.Cnh,
                DataValidadeCnh = m.DataValidadeCnh,
                Telefone = m.Telefone,
                Email = m.Email,
                DataCadastro = m.DataCadastro,
                Ativo = m.Ativo,
                IdEndereco = m.IdEndereco
            });
        }

        public async Task<MotociclistaDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.Motociclistas.FindAsync(id);
            if (entidade == null)
                return null;

            return new MotociclistaDTO
            {
                Id = entidade.Id,
                Nome = entidade.Nome,
                Cpf = entidade.Cpf,
                Cnh = entidade.Cnh,
                DataValidadeCnh = entidade.DataValidadeCnh,
                Telefone = entidade.Telefone,
                Email = entidade.Email,
                DataCadastro = entidade.DataCadastro,
                Ativo = entidade.Ativo,
                IdEndereco = entidade.IdEndereco
            };
        }

        public async Task<MotociclistaDTO> CreateAsync(MotociclistaDTO dto)
        {
            try
            {
                var entidade = new MotociclistaEntity
                {
                    Id = dto.Id,
                    Nome = dto.Nome,
                    Cpf = dto.Cpf,
                    Cnh = dto.Cnh,
                    DataValidadeCnh = dto.DataValidadeCnh,
                    Telefone = dto.Telefone,
                    Email = dto.Email,
                    DataCadastro = dto.DataCadastro,
                    Ativo = dto.Ativo,
                    IdEndereco = dto.IdEndereco
                };

                _context.Motociclistas.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Motociclista: " + ex.Message);
            }
        }

        public async Task<MotociclistaDTO> UpdateAsync(long id, MotociclistaDTO dto)
        {
            try
            {
                var entidade = await _context.Motociclistas.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Nome = dto.Nome;
                entidade.Cpf = dto.Cpf;
                entidade.Cnh = dto.Cnh;
                entidade.DataValidadeCnh = dto.DataValidadeCnh;
                entidade.Telefone = dto.Telefone;
                entidade.Email = dto.Email;
                entidade.DataCadastro = dto.DataCadastro;
                entidade.Ativo = dto.Ativo;
                entidade.IdEndereco = dto.IdEndereco;

                _context.Motociclistas.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Motociclista: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.Motociclistas.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.Motociclistas.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Motociclista: " + ex.Message);
            }
        }

        public async Task<IEnumerable<MotociclistaDTO>> GetByNomeAsync(string nome)
        {
            var lista = await _context.Motociclistas
                .Where(m => m.Nome.ToLower().Contains(nome.ToLower()))
                .ToListAsync();

            return lista.Select(m => new MotociclistaDTO
            {
                Id = m.Id,
                Nome = m.Nome,
                Cpf = m.Cpf,
                Cnh = m.Cnh,
                DataValidadeCnh = m.DataValidadeCnh,
                Telefone = m.Telefone,
                Email = m.Email,
                DataCadastro = m.DataCadastro,
                Ativo = m.Ativo,
                IdEndereco = m.IdEndereco
            });
        }
    }
}
