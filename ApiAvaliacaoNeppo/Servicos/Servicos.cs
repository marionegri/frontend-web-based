using Entidades;
using Microsoft.EntityFrameworkCore;
using Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Servicos
{
    public class Servicos
    {
        private readonly PessoaContext _context;

        public Servicos(PessoaContext context)
        {
            _context = context;

            if (_context.Pessoas.Count() == 0)
            {
                _context.Pessoas.Add(new Pessoa { Id = 1, Nome = "Maria", DataNascimento = Convert.ToDateTime("2018-02-01"), Documento = "123465789", Endereco = "Rua Joao", Sexo = "Feminio" });
                _context.Pessoas.Add(new Pessoa { Id = 2, Nome = "Joao", DataNascimento = Convert.ToDateTime("2009-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 3, Nome = "Joao", DataNascimento = Convert.ToDateTime("2008-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 4, Nome = "Joao", DataNascimento = Convert.ToDateTime("1999-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 5, Nome = "Joao", DataNascimento = Convert.ToDateTime("1998-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 6, Nome = "Joao", DataNascimento = Convert.ToDateTime("1989-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 7, Nome = "Joao", DataNascimento = Convert.ToDateTime("1988-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Masculino" });
                _context.Pessoas.Add(new Pessoa { Id = 8, Nome = "Joao", DataNascimento = Convert.ToDateTime("1979-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Feminio" });
                _context.Pessoas.Add(new Pessoa { Id = 9, Nome = "Joao", DataNascimento = Convert.ToDateTime("1977-02-01"), Documento = "7897846", Endereco = "Rua Teste", Sexo = "Feminio" });
                _context.SaveChanges();
            }
        }

        public List<Pessoa> GetAll(string nome)
        {
            var result = new List<Pessoa>();

            if (string.IsNullOrWhiteSpace(nome))
            {
                result = _context.Pessoas.ToList();
            }
            else
            {
                result = _context.Pessoas.Where(x => x.Nome.ToUpper().Contains(nome.ToUpper())).ToList();
            }

            return result;
        }

        public Pessoa GetById(int id)
        {
            return _context.Pessoas.Where(x => x.Id == id).FirstOrDefault();
        }

        public void Save(Pessoa entity)
        {
            _context.Pessoas.Add(entity);

            _context.SaveChanges();
        }

        public void Update(Pessoa entity)
        {
            _context.Pessoas.Update(entity);

            _context.SaveChanges();
        }

        public void Delete(Pessoa entity)
        {
            _context.Pessoas.Remove(entity);

            _context.SaveChanges();
        }
    }
}
