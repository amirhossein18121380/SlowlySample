using Application.Users.DTOs;
using AutoMapper;
using Domain.Dto.Profile;
using Domain.Models;
using SlowlySimulate.Api.ViewModels.Profile;

namespace SlowlySimulate.Api.Mapping;
public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<GetProfileDto, SlowlyUser>().ReverseMap()
            .ForMember(x => x.BirthDate, y => y.MapFrom(y => y.DateOfBirth));
        //CreateMap<SlowlyUser, GetProfileDto>()
        //    .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
        //    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
        //    .ForMember(dest => dest.SlowlyId, opt => opt.MapFrom(src => src.SlowlyId))
        //    .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.DateOfBirth))
        //    .ForMember(dest => dest.GenderType, opt => opt.MapFrom(src => src.Gender))
        //    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
        //    .ForMember(dest => dest.LetterLength, opt => opt.MapFrom(src => src.LetterLength))
        //    .ForMember(dest => dest.ReplyTime, opt => opt.MapFrom(src => src.ReplyTime))
        //    .ForMember(dest => dest.AboutMe, opt => opt.MapFrom(src => src.AboutMe))
        //    .ForMember(dest => dest.AllowAddMeById, opt => opt.MapFrom(src => src.AllowAddMeById));

        CreateMap<GetProfileViewModel, GetProfileDto>().ReverseMap();
        CreateMap<LanguagesOfUserDto, LanguagesOfUser>().ReverseMap();
        CreateMap<GetProfilePreviewViewModel, ProfilePreviewDto>().ReverseMap();
        CreateMap<ProfilePreviewDto, SlowlyUser>().ReverseMap();
        CreateMap<GetUserMatchingPreferencesDto, MatchingPreferences>().ReverseMap();
        CreateMap<GetUserMatchingPreferencesViewModel, GetUserMatchingPreferencesDto>().ReverseMap();
    }
}