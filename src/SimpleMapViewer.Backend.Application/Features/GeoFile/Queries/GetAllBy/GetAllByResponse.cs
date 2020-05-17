using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllBy {
    public class GetAllByResponse {
        public IList<GeoFileDto> GeoFiles { get; set; }
    }
}