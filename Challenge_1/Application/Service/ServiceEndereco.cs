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
    public class ServiceEndereco : InterfaceEndereco
    {
        private readonly AppDbContext _context;

        public ServiceEndereco(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EnderecoDTO>> GetAllAsync()
        {
            var lista = await _context.Enderecos.ToListAsync();
            return lista.Select(e => new EnderecoDTO
            {
                Id = e.Id,
                Logradouro = e.Logradouro,
                Numero = e.Numero,
                Complemento = e.Complemento,
                Bairro = e.Bairro,
                Cep = e.Cep,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Pais = e.Pais
            });
        }

        public async Task<EnderecoDTO> GetByIdAsync(long id)
        {
            var entidade = await _context.Enderecos.FindAsync(id);
            if (entidade == null)
                return null;

            return new EnderecoDTO
            {
                Id = entidade.Id,
                Logradouro = entidade.Logradouro,
                Numero = entidade.Numero,
                Complemento = entidade.Complemento,
                Bairro = entidade.Bairro,
                Cep = entidade.Cep,
                Cidade = entidade.Cidade,
                Estado = entidade.Estado,
                Pais = entidade.Pais
            };
        }

        public async Task<EnderecoDTO> CreateAsync(EnderecoDTO dto)
        {
            try
            {
                var entidade = new EnderecoEntity
                {
                    Id = dto.Id,
                    Logradouro = dto.Logradouro,
                    Numero = dto.Numero,
                    Complemento = dto.Complemento,
                    Bairro = dto.Bairro,
                    Cep = dto.Cep,
                    Cidade = dto.Cidade,
                    Estado = dto.Estado,
                    Pais = dto.Pais
                };

                _context.Enderecos.Add(entidade);
                await _context.SaveChangesAsync();

                dto.Id = entidade.Id;
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criar Endereco: " + ex.Message);
            }
        }

        public async Task<EnderecoDTO> UpdateAsync(long id, EnderecoDTO dto)
        {
            try
            {
                var entidade = await _context.Enderecos.FindAsync(id);
                if (entidade == null)
                    return null;

                entidade.Logradouro = dto.Logradouro;
                entidade.Numero = dto.Numero;
                entidade.Complemento = dto.Complemento;
                entidade.Bairro = dto.Bairro;
                entidade.Cep = dto.Cep;
                entidade.Cidade = dto.Cidade;
                entidade.Estado = dto.Estado;
                entidade.Pais = dto.Pais;

                _context.Enderecos.Update(entidade);
                await _context.SaveChangesAsync();

                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao atualizar Endereco: " + ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                var entidade = await _context.Enderecos.FindAsync(id);
                if (entidade == null)
                    return false;

                _context.Enderecos.Remove(entidade);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao excluir Endereco: " + ex.Message);
            }
        }

        public async Task<IEnumerable<EnderecoDTO>> GetByCidadeAsync(string cidade)
        {
            var lista = await _context.Enderecos
                .Where(e => e.Cidade.ToLower().Contains(cidade.ToLower()))
                .ToListAsync();

            return lista.Select(e => new EnderecoDTO
            {
                Id = e.Id,
                Logradouro = e.Logradouro,
                Numero = e.Numero,
                Complemento = e.Complemento,
                Bairro = e.Bairro,
                Cep = e.Cep,
                Cidade = e.Cidade,
                Estado = e.Estado,
                Pais = e.Pais
            });
        }
    }
}