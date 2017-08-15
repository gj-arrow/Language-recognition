using System;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Work.BLL.DTO;
using Work.Models;

namespace Work.App_Start
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration MapperConfiguration;
        public static void RegisterMappings()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TopUserDTO, TopUserViewModel>().ReverseMap();
            });
        }
    }
}