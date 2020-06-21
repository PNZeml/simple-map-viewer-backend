using MediatR;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.DeleteGeoFile {
    internal class DeleteGeoFileRequest : IRequest {
        public long GeoFileId { get; set; }
    }
}