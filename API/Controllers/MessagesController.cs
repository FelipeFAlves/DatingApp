using System;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize]
public class MessagesController(IMessageRepository messageRepository, 
    IUserRepository userRepository, 
    IMapper mapper) : BaseAPIController
{
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto){
        var username = User.GetUsername();

        if(username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("Não é possível mandar mensagem para si mesmo");

        var sender = await userRepository.GetUserByUsernameAsync(username);
        var recipient = await userRepository.GetUserByUsernameAsync(createMessageDto.RecipientUsername);

        if(recipient == null || sender == null) return BadRequest("Não é possível mandar mensagens no momento");

        var message = new Message{
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };
        messageRepository.AddMessage(message);
        
        if( await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));
        return BadRequest("Falha ao salvar mensagem");
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>>
        GetMessagesForUser([FromQuery] MessageParams messageParams){
            messageParams.Username  = User.GetUsername();

            var messages = await messageRepository.GetMessagesForUser(messageParams);
            
            Response.AddPaginationHeader(messages);
            return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username){
        var currentUsername = User.GetUsername();

        return Ok(await messageRepository.GetMessageThread(currentUsername, username));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id){
        var username = User.GetUsername();

        var message = await messageRepository.GetMessage(id);

        if(message == null) return BadRequest("Não é possível deletar essa mensagem");

        if(message.SenderUsername != username 
        && message.RecipientUsername != username) return Forbid();

        if(message.SenderUsername == username) message.SenderDeleted = true;
        if(message.RecipientUsername == username) message.RecipientDeleted = true;

        if(message is {SenderDeleted:true, RecipientDeleted: true}) {
            messageRepository.DeleteMessage(message);
        }

        if( await messageRepository.SaveAllAsync()) return Ok();

        return BadRequest ("Ocorreu algum problema ao se deletar a mensagem");
    }

}
