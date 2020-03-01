using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Dtos;
using ServerApp.Helper;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
	[ServiceFilter(typeof(LogUserActivity))]
	[Authorize]
	[Route("api/users/{userId}/[controller]")]
	[ApiController]
	public class MessagesController : ControllerBase
	{
		private readonly IDatingRepository _repos;
		private readonly IMapper _mapper;

		// GET
		public MessagesController(IDatingRepository repos, IMapper mapper)
		{
			_repos = repos;
			_mapper = mapper;
		}

		[HttpGet("{id}", Name = "GetMessage")]
		public async Task<IActionResult> GetMessage(int userId, int recipientId)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var messageFromRepos = await _repos.GetMessage(recipientId);
			if (messageFromRepos == null)
				return NotFound();
			return Ok(messageFromRepos);
		}

		[HttpGet]
		public async Task<IActionResult> GetMessages(int userId, [FromQuery]MessagesParam messagesParam)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			messagesParam.UserId = userId;
			var messageFromRepos = await _repos.GetMessagesForUser(messagesParam);
			if (messagesParam == null)
				return BadRequest("no message found");
			var messageToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepos);
			Response.AddPagination(messageFromRepos.CurrentPage, messageFromRepos.PageSize, 
				messageFromRepos.TotalItems, messageFromRepos.TotalPages);
			return Ok(messageToReturn);
		}

		[HttpGet("thread/{recipientId}")]
		public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var messagFromRepos = await _repos.GetMessageThread(userId, recipientId);
			var messageToReturn = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagFromRepos);
			return Ok(messageToReturn);
		}
		[HttpPost]
		public async Task<IActionResult> AddMessage(int userId, MessageForCreationDto messageForCreationDto)
		{
			var sender = await _repos.GetUser(userId);
			if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();

			messageForCreationDto.SenderId = userId;
			var recipient = await _repos.GetUser(messageForCreationDto.RecipientId);
			if (recipient == null)
				return BadRequest("User not found");
			var message = _mapper.Map<Message>(messageForCreationDto);
			await _repos.Add(message);
			
			if (await _repos.SaveAll())
			{
				var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
				return CreatedAtRoute("GetMessage", new {id = message.Id}, messageToReturn);
			}

			return BadRequest("Message failed");

		}

		[HttpPost("{id}")]
		public async Task<IActionResult> DeleteMessage(int id, int userId)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			
				var messageFromRepos = await _repos.GetMessage(id);
				if (messageFromRepos.SenderId == userId)
				{
					messageFromRepos.SenderDelete = true;
				}

				if (messageFromRepos.RecipientId == userId)
				{
					messageFromRepos.RecipientDelete = true;
				}

				if (messageFromRepos.RecipientDelete == true && messageFromRepos.SenderDelete == true)
				{
					_repos.Delete(messageFromRepos);
					
				}

				if (await _repos.SaveAll())
				{
					return NoContent();
				}
			
			return BadRequest("Something went wrong. Please try again");
		}

		[HttpPost("{id}/read")]
		public async Task<IActionResult> MarkMessageAsRead(int userId, int id)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var message = await _repos.GetMessage(id);
			if (message.RecipientId != userId)
			   return Unauthorized();
			message.IsRead = true;
			await _repos.SaveAll();
			return NoContent();
		}
	}
}