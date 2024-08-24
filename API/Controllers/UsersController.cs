﻿using API.Data;
using API.DTO;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository) : BaseAPIController
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
}