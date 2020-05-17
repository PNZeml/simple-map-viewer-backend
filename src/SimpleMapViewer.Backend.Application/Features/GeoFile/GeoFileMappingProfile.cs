using AutoMapper;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile {
    internal class GeoFileMappingProfile : Profile {
        public GeoFileMappingProfile() {
            CreateMap<Domain.Entities.GeoFile, GeoFileDto>();
        }
    }
}