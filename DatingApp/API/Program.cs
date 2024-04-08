using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => { // Confugurar acesso ao bano de dados
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    //Diz ao opt que vai ser utilizado o Sqlite
    //e o resto é o caminho para o banco de dados (ou configuração)
}); 

builder.Services.AddCors(); // Permisão para mandar dados
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// app.UseAuthorization();

app.MapControllers();

app.Run();
