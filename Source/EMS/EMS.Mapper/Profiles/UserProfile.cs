using AutoMapper;
using EMS.DataAccess.Models;
using EMS.DTO;

namespace EMS.Mapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<RoleDTO, Role>();
            CreateMap<Role, RoleDTO>();

            CreateMap<ModuleDTO, Module>();
            CreateMap<Module, ModuleDTO>();

            CreateMap<UserStatusDTO, UserStatus>();
            CreateMap<UserStatus, UserStatusDTO>();

           

            // Mapping for DisplayDTO
            CreateMap<User, DisplayDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Status, DisplayDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UserStatus, DisplayDTO>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));


            // Mapping for AllUsersDTO
            CreateMap<User, AllUsersDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name));

            CreateMap<Status, AllUsersDTO>()
                .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => src.Name));

            CreateMap<UserStatus, AllUsersDTO>()
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Note));
        }
    }
}
