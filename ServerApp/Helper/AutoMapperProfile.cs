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
			CreateMap<Photo, PhotoToReturnDto>();
			CreateMap<PhotoForCreationDto, Photo>();
			CreateMap<UserForRegisterDto, User>();
			CreateMap<MessageForCreationDto, Message>().ReverseMap();
			CreateMap<Message, MessageToReturnDto>()
				.ForMember(dest => dest.SenderPhotoUrl, opt => opt
					.MapFrom(u => u.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
				.ForMember(dest => dest.RecipientPhotoUrl, opt => opt
					.MapFrom(u => u.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
		}
	}
}