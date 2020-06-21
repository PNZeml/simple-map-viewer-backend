using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsers {
    public class GetAllUsersResponse {
        public IList<UserOutDto> Users { get; set; }
    }
}