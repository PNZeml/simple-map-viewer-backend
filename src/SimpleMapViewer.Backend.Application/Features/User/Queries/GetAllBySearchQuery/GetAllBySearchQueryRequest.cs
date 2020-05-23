using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllBySearchQuery {
    [UnitOfWorkRequired]
    public class GetAllBySearchQueryRequest : IRequest<GetAllBySearchQueryResponse> {
        public string SearchQuery { get; set; }
    }
}