using System.Linq;
using AutoMapper;
using ServerApp.Dtos;
using ServerApp.Models;

namespace ServerApp.Helper
{
	public class AutoMapperProfiles:Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<User, UserListDto>()
				.ForMember(dest=>dest.PhotoUrl, opt=>
					opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url))
				.ForMember(dest=>dest.Age, opt=>
					opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
			CreateMap<User, UserDetailsDto>()
				.ForMember(dest=>dest.PhotoUrl, opt=>
					opt.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url))
				.ForMember(dest=>dest.Age, opt=>
					opt.MapFrom(src=>src.DateOfBirth.CalculateAge()));
			CreateMap<Photo, PhotoForDetailDto>();
			CreateMap<UserForUpdateDto, User>();
		}
	}
}