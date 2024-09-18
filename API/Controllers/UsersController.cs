using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository,IMapper mapper) : BaseAPIController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers(){
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }

    [HttpGet("{username}")] // /api/users/1
    public async Task<ActionResult<MemberDTO>> GetUser(string username){
        var user =await userRepository.GetMemberAsync(username);

        if(user == null) return NotFound();
        return user;
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null) return BadRequest("Nenhum usuário com esse nome foi achado no token");
        var user = await userRepository.GetUserByUsernameAsync(username);
        if(user == null) return BadRequest("Não foi possível achar usuário");

        mapper.Map(memberUpdateDto,user);

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Falha ao atualizar o usuário");
    }
}