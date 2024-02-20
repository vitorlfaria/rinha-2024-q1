using Microsoft.EntityFrameworkCore;
using rinha_2024_q1.Entities;

namespace rinha_2024_q1.Data;

public class RinhaDbContext(DbContextOptions<RinhaDbContext> options) : DbContext(options)
{
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Transacao> Transacoes => Set<Transacao>();
}
