using MediatR;
using SimpleMapViewer.Infrastructure.Attributes;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsers {
    [UnitOfWorkRequired]
    public class GetAllUsersRequest : IRequest<GetAllUsersResponse> {
        public string SearchString { get; set; }
    }
}