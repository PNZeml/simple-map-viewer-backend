using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllByGeoFile {
    [UnitOfWorkRequired]
    public class GetAllByGeoFileRequest : IRequest<GetAllByGeoFileResponse> {
        public long GeoFileId { get; set; }
    }
}