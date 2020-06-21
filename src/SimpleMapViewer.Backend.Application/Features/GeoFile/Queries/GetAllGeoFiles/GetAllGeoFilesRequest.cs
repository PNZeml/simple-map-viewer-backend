using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllGeoFiles {
    [UnitOfWorkRequired]
    internal class GetAllGeoFilesRequest : IRequest<GetAllGeoFilesResponse> {
        public bool Shared { get; set; }
    }
}