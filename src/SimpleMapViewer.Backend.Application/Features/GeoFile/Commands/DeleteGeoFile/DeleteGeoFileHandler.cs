using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.DeleteGeoFile {
    internal class DeleteGeoFileHandler : AsyncRequestHandler<DeleteGeoFileRequest> {
        protected override async Task Handle(
            DeleteGeoFileRequest request,
            CancellationToken cancellationToken
        ) {
            
        }
    }
}