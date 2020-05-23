using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAll {
    public class GetAllResponse {
        public IList<GeoFileDto> GeoFiles { get; set; }
    }
}