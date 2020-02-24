using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServerApp.Dtos;
using ServerApp.Helper;
using ServerApp.Models;
using ServerApp.Services;

namespace ServerApp.Controllers
{
	[Route("api/users/{userId}/photos")]
	[ApiController]
	public class PhotosController : ControllerBase
	{
		private readonly IDatingRepository _repos;
		private readonly IMapper _mapper;
		private readonly IOptions<CloudinarySettings> _cloudinaryConfig;

		private Cloudinary _cloudinary;

		public PhotosController(IDatingRepository repos, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
		{
			_repos = repos;
			_mapper = mapper;
			_cloudinaryConfig = cloudinaryConfig;

			Account account = new Account(
				_cloudinaryConfig.Value.CloudName = "dgvvke9vc",
				_cloudinaryConfig.Value.ApiKey= "173911215941576",
				_cloudinaryConfig.Value.ApiSecret = "zhg4ejyvbNbBXH5Sk7exxbn8CKA"
				);
			
			_cloudinary = new Cloudinary(account);
				
		}

		[HttpGet("{id}", Name = "GetPhoto")]
		public async Task<IActionResult> GetPhoto(int id)
		{
			var photoFromRepos = await _repos.GetPhoto(id);
			var photoToReturn = _mapper.Map<PhotoToReturnDto>(photoFromRepos);
			return Ok(photoToReturn);
		}

		[HttpPost()]
		public async Task<IActionResult> AddPhoto(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var userFromRepos = await _repos.GetUser(userId);
			var file = photoForCreationDto.File;
			var uploadResult = new ImageUploadResult();
			if (file.Length > 0)
			{
				using (var stream = file.OpenReadStream())
				{
					var uploadParams = new ImageUploadParams
					{
						File = new FileDescription(file.Name, stream),
						Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
					};
					uploadResult = _cloudinary.Upload(uploadParams);
				}
			}
         photoForCreationDto.Url = uploadResult.Uri.ToString();
			photoForCreationDto.PublicId = uploadResult.PublicId;
			Photo photo = _mapper.Map<Photo>(photoForCreationDto);
			if (!userFromRepos.Photos.Any(x => x.IsMain))
				photo.IsMain = true;
			userFromRepos.Photos.Add(photo);
			
			if (await _repos.SaveAll())
			{
				var photoToReturn = _mapper.Map<PhotoToReturnDto>(photo);
				return CreatedAtRoute("GetPhoto", new{id = photo.Id}, photoToReturn);
			}

			return BadRequest();
		}

		[HttpPost("{id}/setMain")]
		public async Task<IActionResult> SetMainPhoto(int userId, int id)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var photoFromRepos = await _repos.GetPhoto(id);
			if (photoFromRepos == null)
				return BadRequest("Photo does not exist");
			if (photoFromRepos.IsMain)
				return BadRequest("Photo is already the main photo");
			var currentMainPhoto = await _repos.GetMainPhoto(userId);
			currentMainPhoto.IsMain = false;
			photoFromRepos.IsMain = true;
			if (await _repos.SaveAll())
				return NoContent();
			return BadRequest("Unable to set main photo");
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePhoto(int userId, int id)
		{
			if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
				return Unauthorized();
			var photoFromRepos = await _repos.GetPhoto(id);
			if (photoFromRepos == null)
				return BadRequest("Photo does not exist");
			if (photoFromRepos.IsMain)
				return BadRequest("Main photo cannot be deleted");
			if (photoFromRepos.PublicId != null)
			{
				var deleteParams = new DeletionParams(photoFromRepos.PublicId);
				var result = await _cloudinary.DestroyAsync(deleteParams);
				if (result.Result == "ok")
				{
					_repos.Delete(photoFromRepos);
				}
			}

			if (photoFromRepos.PublicId == null)
			{
				_repos.Delete(photoFromRepos);
			}

			if (await _repos.SaveAll())
			{
				return Ok();
			}

			return BadRequest("Could not delete this photo");
		}
	}
}