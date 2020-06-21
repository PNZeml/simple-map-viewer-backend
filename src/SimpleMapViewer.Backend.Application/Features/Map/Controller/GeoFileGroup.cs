using System.Collections.Generic;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Map.Controller {
    public class GeoFileGroup {
        public HashSet<UserInDto> Users { get; }
        public long GeoFileId { get; }

        public GeoFileGroup(long geoFileId) {
            Users = new HashSet<UserInDto>();
            GeoFileId = geoFileId;
        }

        public override string ToString() => GeoFileId.ToString();
    }
}