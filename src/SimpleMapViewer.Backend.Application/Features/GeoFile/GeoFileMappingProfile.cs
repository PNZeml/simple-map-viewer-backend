using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    internal class GeoFileMappingProfile : Profile {
        public GeoFileMappingProfile() {
            CreateMap<Domain.Entities.User, UserOutDto>();
            CreateMap<Domain.Entities.GeoFile, GeoFileOutDto>();
            CreateMap<GeoFileActivityRecord, GeoFileActivityRecordOutDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(x => x.User.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(x => x.User.Name))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(x => x.User.Avatar));
        }
    }
}