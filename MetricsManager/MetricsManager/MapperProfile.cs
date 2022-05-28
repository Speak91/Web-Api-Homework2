using AutoMapper;
using MetricsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsManager
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //#region Agents
            CreateMap<AgentInfoResponse, AgentInfo>().
                ForMember(x => x.AgentAddress,
                opt => opt.MapFrom(src => new Uri(src.Uri))).
                ForMember(x => x.AgentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(x => x.Enable, opt => opt.MapFrom(src => Convert.ToBoolean(src.IsEnabled)));
            //#endregion
        }
    }
}
