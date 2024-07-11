using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options) //DbCOntext é uma sessão com o BD
{
    public DbSet<AppUser> Users { get; set; } // Tabela "User" com atributos da classe "AppUser"
}
