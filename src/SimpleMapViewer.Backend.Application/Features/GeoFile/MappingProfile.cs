using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    internal class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<Domain.Entities.GeoFile, GeoFileDto>();
        }
    }
}