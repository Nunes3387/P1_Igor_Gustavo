using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using P1_Igor_Gustavo.Models;

namespace P1_Igor_Gustavo.Data
{
    public class P1_Igor_GustavoContext : DbContext
    {
        public P1_Igor_GustavoContext (DbContextOptions<P1_Igor_GustavoContext> options)
            : base(options)
        {
        }

        public DbSet<P1_Igor_Gustavo.Models.Cliente> Cliente { get; set; } = default!;

        public DbSet<P1_Igor_Gustavo.Models.Fornecedor>? Fornecedor { get; set; }

        public DbSet<P1_Igor_Gustavo.Models.Funcionario>? Funcionario { get; set; }

        public DbSet<P1_Igor_Gustavo.Models.Estoque>? Estoque { get; set; }

        public DbSet<P1_Igor_Gustavo.Models.Venda>? Venda { get; set; }

        public DbSet<P1_Igor_Gustavo.Models.Produto>? Produto { get; set; }
    }
}
