using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsersBy {
    [UnitOfWorkRequired]
    public class GetAllUsersByRequest : IRequest<GetAllUsersByResponse> {
        public string SearchQuery { get; set; }
    }
}