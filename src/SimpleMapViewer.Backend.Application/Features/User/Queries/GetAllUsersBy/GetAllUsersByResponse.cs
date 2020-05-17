using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.User.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsersBy {
    public class GetAllUsersByResponse {
        public IList<UserDto> Users { get; set; }
    }
}