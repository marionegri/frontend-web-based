using Entidades;
using Microsoft.EntityFrameworkCore;

namespace Repositorio
{
    public class PessoaContext : DbContext
    {
        public PessoaContext(DbContextOptions<PessoaContext> options) : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas { get; set; }
    }
}
