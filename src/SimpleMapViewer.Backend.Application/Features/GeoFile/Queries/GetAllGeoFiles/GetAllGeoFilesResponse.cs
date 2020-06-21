using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllGeoFiles {
    internal class GetAllGeoFilesResponse {
        public IList<GeoFileOutDto> GeoFiles { get; set; }
    }
}