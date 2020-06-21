using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetGeoFile {
    [UnitOfWorkRequired]
    [ValidationRequired]
    internal class GetGeoFileRequest : IRequest<GetGeoFileResponse> {
        public long GeoFileId { get; set; }
    }
}