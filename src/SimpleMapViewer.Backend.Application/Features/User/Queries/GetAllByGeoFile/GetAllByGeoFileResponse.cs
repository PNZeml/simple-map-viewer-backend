using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllByGeoFile {
    public class GetAllByGeoFileResponse {
        public IList<UserAccessTypeDto> Users { get; set; }
    }
}