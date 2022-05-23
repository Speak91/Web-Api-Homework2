using AutoMapper;
using MetricsAgent.Models;
using MetricsAgent.Models.DTO;
using MetricsAgent.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region CPU
            CreateMap<CpuMetric, CpuMetricDto>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));

            CreateMap<CpuMetricCreateRequest, CpuMetric>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => src.Time.TotalSeconds));
            #endregion

            #region DOT NET
            CreateMap<DotNetMetric, DotNetMetricDto>().
               ForMember(x => x.Time,
               opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));

            CreateMap<DotNetMetricCreateRequest, DotNetMetric>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => src.Time.TotalSeconds));
            #endregion

            #region HDD
            CreateMap<HddMetric, HddMetricDto>().
               ForMember(x => x.Time,
               opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));

            CreateMap<HddMetricCreateRequest, HddMetric>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => src.Time.TotalSeconds));
            #endregion

            #region Network
            CreateMap<NetworkMetric, NetworkMetricDto>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));

            CreateMap<NetworkMetricCreateRequest, NetworkMetric>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => src.Time.TotalSeconds));
            #endregion

            #region Ram
            CreateMap<RamMetric, RamMetricDto>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.Time)));

            CreateMap<RamMetricCreateRequest, RamMetric>().
                ForMember(x => x.Time,
                opt => opt.MapFrom(src => src.Time.TotalSeconds));
            #endregion
        }
    }
}
