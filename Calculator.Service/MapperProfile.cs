using AutoMapper;
using Calculator.Model;
using Calculator.Model.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Service
{
    public class MapperConfig : Profile
    {
        public MapperConfig() { 
          CreateMap<OperationDto, OperationEntity>().ReverseMap();
        }
    }
}
