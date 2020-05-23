using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAll {
    [UnitOfWorkRequired]
    public class GetAllRequest : IRequest<GetAllResponse> {
        public long UserId { get; set; }
        public bool Shared { get; set; }
    }
}