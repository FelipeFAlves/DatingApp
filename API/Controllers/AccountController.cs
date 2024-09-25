using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService):BaseAPIController
{
    [HttpPost("register")] // /api/account/register
    public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO){
        if(await UserExists(registerDTO.Username)) return BadRequest("Usuário já existe");
        return Ok();
        // using var hmac = new HMACSHA512();

        // var user = new AppUser{
        //     UserName = registerDTO.Username,
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
        //     PasswordSalt = hmac.Key
        // };

        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        // return new UserDTO {
        //     Username =  user.UserName,
        //     Token =  tokenService.CreateToken(user)
        // };
    }

    [HttpPost("login")] //api/account/login
    public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO){
        var user = await context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(x => x.UserName.ToLower() == 
        loginDTO.Username.ToLower());

        if(user == null) return Unauthorized("Usuário não existe");

        var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for (int i = 0; i < computedHash.Length; i++){
            if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Senha inválida");
        }

        return new UserDTO{
            Username = user.UserName,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

    private async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
