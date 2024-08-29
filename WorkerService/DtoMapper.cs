using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerService.Models;

namespace WorkerService
{
    public class DtoMapper : Profile
    {
        public DtoMapper() { 
            CreateMap<UserDto,User>().ReverseMap();
        }
    }
}
