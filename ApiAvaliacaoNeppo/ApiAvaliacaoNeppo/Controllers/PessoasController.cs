using System;
using System.Collections.Generic;
using System.Linq;
using ApiAvaliacaoNeppo.Response;
using ApiAvaliacaoNeppo.ViewModel;
using Entidades;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiAvaliacaoNeppo.Controllers
{
    [Produces("application/json")]
    [EnableCors("Policy")]
    public class PessoasController : Controller
    {
        private readonly Servicos.Servicos _servicos;

        public PessoasController(Servicos.Servicos servicos)
        {
            _servicos = servicos;
        }

        /// <summary>
        /// Retorna lista de todas as Pessoas
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        [HttpGet("pessoas")]
        [ProducesResponseType(typeof(Response<Pessoa>), 200)]
        [ProducesResponseType(typeof(Response<Pessoa>), 404)]
        [ProducesResponseType(typeof(Response<Pessoa>), 500)]
        public Response<Pessoa> GetAll(string nome)
        {
            try
            {
                var result = _servicos.GetAll(nome);
                
                return new Response<Pessoa>()
                {
                    Erro = false,
                    DataLista = result
                };
            }
            catch (Exception ex)
            {
                return new Response<Pessoa>()
                {
                    Erro = true,
                    Mensagem = ex.Message
                };
            }
        }

        /// <summary>
        /// Grafico da faixa de idades das pessoas, faixas: ["0 a 9", "10 a 19", "20 a 29", "30 a 39", "Maior que 40"]
        /// </summary>
        /// <returns></returns>
        [HttpGet("faixa_idades")]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 404)]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 500)]
        public IEnumerable<Relatorio> GetFaixaIdade()
        {
            try
            {
                var result = new List<Relatorio>();

                var datas = _servicos.GetAll(null).Select(x => x.DataNascimento).ToList();

                var inicio = 0;
                var fim = 9;
                for (int i = 0; i < 5; i++)
                {
                    if (inicio == 40)
                    {
                        result.Add(new Relatorio
                        {
                            Descricao = "Maior que " + inicio.ToString(),
                            Qtde = datas.Where(x => (DateTime.Now.Year - x.Year) > inicio).Count()
                        });
                    }
                    else
                    {
                        result.Add(new Relatorio
                        {
                            Descricao = inicio.ToString() + " a " + fim.ToString(),
                            Qtde = datas.Where(x => (DateTime.Now.Year - x.Year) >= inicio && (DateTime.Now.Year - x.Year) <= fim).Count()
                        });
                    }

                    inicio += 10;
                    fim += 10;
                }
                
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Grafico contendo quantidade de pessoas que tenham sexo masculino e feminino
        /// </summary>
        /// <returns></returns>
        [HttpGet("qtd_sexos")]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 200)]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 404)]
        [ProducesResponseType(typeof(IEnumerable<Relatorio>), 500)]
        public IEnumerable<Relatorio> GetQtdSexo()
        {
            try
            {
                return _servicos.GetAll(null)
                                .GroupBy(a => a.Sexo)
                                .Select(g => new Relatorio { Descricao = g.Key, Qtde = g.Count() }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Retorna pessoa por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("pessoas/{id}")]
        [ProducesResponseType(typeof(Response<Pessoa>), 200)]
        [ProducesResponseType(typeof(Response<Pessoa>), 404)]
        [ProducesResponseType(typeof(Response<Pessoa>), 500)]
        public Response<Pessoa> GetById(int id)
        {
            try
            {
                var result = _servicos.GetById(id);

                return new Response<Pessoa>()
                {
                    Erro = false,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new Response<Pessoa>()
                {
                    Erro = true,
                    Mensagem = ex.Message
                };
            }
        }

        /// <summary>
        /// Insere pessoa
        /// </summary>
        /// <param name="viewModel"></param>
        [HttpPost("pessoas")]
        [ProducesResponseType(typeof(Response<Pessoa>), 200)]
        [ProducesResponseType(typeof(Response<Pessoa>), 500)]
        public Response<Pessoa> Post([FromBody]PessoaViewModel viewModel)
        {
            try
            {
                var entity = new Pessoa
                {
                    Id = (_servicos.GetAll(null).Max(x => x.Id) + 1),
                    Nome = viewModel.Nome,
                    DataNascimento = viewModel.DataNascimento,
                    Documento = viewModel.Documento,
                    Endereco = viewModel.Endereco,
                    Sexo = viewModel.Sexo
                };

                _servicos.Save(entity);

                return new Response<Pessoa>()
                {
                    Erro = false,
                    Data = entity
                };
            }
            catch (Exception ex)
            {
                return new Response<Pessoa>()
                {
                    Erro = true,
                    Mensagem = ex.Message
                };
            }
        }

        /// <summary>
        /// Edita pessoa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPut("pessoas/{id}")]
        [ProducesResponseType(typeof(Response<Pessoa>), 200)]
        [ProducesResponseType(typeof(Response<Pessoa>), 404)]
        [ProducesResponseType(typeof(Response<Pessoa>), 500)]
        public Response<Pessoa> Put(int id, [FromBody]PessoaViewModel viewModel)
        {
            try
            {
                var entity = _servicos.GetById(id);
                if (entity != null)
                {
                    entity.Nome = viewModel.Nome;
                    entity.DataNascimento = viewModel.DataNascimento;
                    entity.Documento = viewModel.Documento;
                    entity.Endereco = viewModel.Endereco;
                    entity.Sexo = viewModel.Sexo;

                    _servicos.Update(entity);

                    return new Response<Pessoa>()
                    {
                        Erro = false,
                        Data = entity
                    };
                }
                else
                {
                    throw new Exception("Pessoa não encontrado!");
                }
            }
            catch (Exception ex)
            {
                return new Response<Pessoa>()
                {
                    Erro = true,
                    Mensagem = ex.Message
                };
            }
        }

        /// <summary>
        /// Deleta pessoa
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("pessoas/{id}")]
        [ProducesResponseType(typeof(Response<bool>), 200)]
        [ProducesResponseType(typeof(Response<bool>), 404)]
        [ProducesResponseType(typeof(Response<bool>), 500)]
        public Response<bool> Delete(int id)
        {
            try
            {
                var entity = _servicos.GetById(id);
                if (entity == null)
                {
                    throw new Exception("Pessoa não encontrado!");
                }

                _servicos.Delete(entity);

                return new Response<bool>()
                {
                    Erro = false,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new Response<bool>()
                {
                    Erro = true,
                    Mensagem = ex.Message
                };
            }
        }
    }
}
