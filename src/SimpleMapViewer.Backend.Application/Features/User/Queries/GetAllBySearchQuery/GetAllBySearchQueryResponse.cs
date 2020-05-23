using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllBySearchQuery {
    public class GetAllBySearchQueryResponse {
        public IList<UserDto> Users { get; set; }
    }
}