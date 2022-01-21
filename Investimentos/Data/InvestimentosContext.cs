using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Investimentos.Models;

namespace Investimentos.Data
{
    public class InvestimentosContext : DbContext
    {
        public InvestimentosContext (DbContextOptions<InvestimentosContext> options)
            : base(options)
        {
        }

        public DbSet<TipoPapel> TipoPapel { get; set; }
        public DbSet<Papel> Papel { get; set; }
        public DbSet<Historico> Historico { get; set; }
        public DbSet<Carteira> Carteira { get; set; }
    }
}
