using AutoMapper;
using Common.DTOs;
using Persistence.Models;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDTO>();
    }
}