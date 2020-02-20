using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerApp.Dtos;
using ServerApp.Services;

namespace ServerApp.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	
	public class UsersController : ControllerBase
	{
		private readonly IDatingRepository _repos;
		private readonly IMapper _mapper;

		public UsersController(IDatingRepository repos, IMapper mapper)
		{
			_repos = repos;
			_mapper = mapper;
		}

		[HttpGet]
		public async Task<IActionResult> GetUsers()
		{
			var users = await _repos.GetUsers();
			var usersToReturn = _mapper.Map<IEnumerable<UserListDto>>(users);
			return Ok(usersToReturn);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUser(int id)
		{
			var user = await _repos.GetUser(id);
			var userToReturn = _mapper.Map<UserDetailsDto>(user);
			return Ok(userToReturn);
		}
	}
}