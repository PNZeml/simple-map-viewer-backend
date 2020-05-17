using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllBy {
    [UnitOfWorkRequired]
    public class GetAllByRequest : IRequest<GetAllByResponse> {
        public long UserId { get; set; }
        public bool IsSharedRequested { get; set; }
    }
}