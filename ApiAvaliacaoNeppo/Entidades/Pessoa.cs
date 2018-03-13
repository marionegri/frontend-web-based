using System;

namespace Entidades
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Documento { get; set; }
        public string Sexo { get; set; }
        public string Endereco { get; set; }
    }
}
