using AutoMapper;
using sprintFlow.Domain.Entities;

namespace sprintFlow.Application.Users.Dto;

public class UserProfile : Profile
{
    public UserProfile()
    {
       CreateMap<User, UserDto>();
    }

}
