using AutoMapper;
using quan_ly_kho.DataBase.System;
using quan_ly_kho.common.Models.Users;

namespace quan_ly_kho.common.Helpers
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