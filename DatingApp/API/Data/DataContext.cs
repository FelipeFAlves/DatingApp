using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options){} // Constructor

    public DbSet<AppUser> Users {get;set;} // AppUser é a "classe"(atributos) da tabela e Users o nome da tabela
}
