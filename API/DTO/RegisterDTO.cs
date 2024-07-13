using System.ComponentModel.DataAnnotations;

namespace API;

public class RegisterDTO
{
    [MaxLength(100)]
    public required string Username { get; set; }
    public required string Password { get; set; }
}
