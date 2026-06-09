// importando Entity Framework Core para trabalhar com banco de dados
using Microsoft.EntityFrameworkCore;
using TSEA.API.Models;

// classe responsável pela conexão com o banco de dados
public class AppDbContext : DbContext
{
    // construtor que recebe as configurações do banco
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // tabela de usuários no banco
    public DbSet<Usuario> Usuarios { get; set; }

    // tabela de leituras no banco
    public DbSet<Leitura> Leituras { get; set; }
}