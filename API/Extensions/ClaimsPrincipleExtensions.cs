using System;
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static string GetUsername(this ClaimsPrincipal user){
        var username = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
            throw new Exception("Não é possível pegar o username do token"); 
        return username;
    }
}
