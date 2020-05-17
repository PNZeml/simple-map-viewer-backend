using MediatR;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Share {
    public class ShareRequest : IRequest {
        public long UserId { get; set; }
        public long GeoFileId { get; set; }
        public ShareOptionsDto ShareOptions { get; set; }
    }
}