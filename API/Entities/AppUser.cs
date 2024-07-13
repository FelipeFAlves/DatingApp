namespace API.Entities;

public class AppUser // Classe
{
    public int Id { get; set; } // Atributo
    public required string UserName { get; set; } // Atributo
    public required byte[] PasswordHash { get; set; }

    public required byte[] PasswordSalt { get; set; }
}