using System.Security.Claims;
using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository,IMapper mapper,IPhotoService photoService) : BaseAPIController
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

        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        if(user == null) return BadRequest("Não foi possível achar usuário");

        mapper.Map(memberUpdateDto,user);

        if(await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Falha ao atualizar o usuário");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file){
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if(user == null) return BadRequest("Não foi possível atualizar o usuário");

        var result = await photoService.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);
        if(await userRepository.SaveAllAsync()) 
            return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, mapper.Map<PhotoDto>(photo));

        return BadRequest("Problema ao se adicionar a foto");
    }
}