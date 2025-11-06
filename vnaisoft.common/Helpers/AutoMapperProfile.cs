using AutoMapper;
using vnaisoft.DataBase.System;
using vnaisoft.common.Models.Users;

namespace vnaisoft.common.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserModel>();
            CreateMap<RegisterModel, User>();
            CreateMap<UpdateModel, User>();
        }
    }
}