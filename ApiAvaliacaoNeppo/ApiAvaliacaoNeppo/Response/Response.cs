using System.Collections.Generic;

namespace ApiAvaliacaoNeppo.Response
{
    public class Response<T>
    {
        public bool Erro { get; set; }
        public string Mensagem { get; set; }
        public IEnumerable<T> DataLista { get; set; }
        public T Data { get; set; }
    }
}
