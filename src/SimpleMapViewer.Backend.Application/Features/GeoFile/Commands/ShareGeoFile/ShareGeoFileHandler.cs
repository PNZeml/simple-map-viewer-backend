using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.ShareGeoFile {
    internal class ShareGeoFileHandler : AsyncRequestHandler<ShareGeoFileRequest> {
        private readonly ISession _session;
        private readonly IMediator _mediator;
        
        public ShareGeoFileHandler(IUnitOfWork<ISession> unitOfWork, IMediator mediator) {
            _session = unitOfWork.Source;
            _mediator = mediator;
        }
        
        protected override async Task Handle(
            ShareGeoFileRequest geoFileRequest,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                geoFileRequest.GeoFileId,
                cancellationToken
            );
            foreach (var userId in geoFileRequest.UserIds) {
                if (cancellationToken.IsCancellationRequested) break;
    
                var user = await _session.GetAsync<Domain.Entities.User>(
                    userId,
                    cancellationToken
                );
                var userGeoFileRelation = await _session.Query<UserGeoFileRelation>()
                    .FirstOrDefaultAsync(
                        x => x.User == user && x.GeoFile == geoFile,
                        cancellationToken
                    );
                if (userGeoFileRelation != null) {
                    userGeoFileRelation.AccessType = geoFileRequest.AccessType;
                    continue;
                }

                userGeoFileRelation = new UserGeoFileRelation {
                    GeoFile = geoFile,
                    User = user,
                    AccessType = geoFileRequest.AccessType
                };
                await _session.SaveAsync(userGeoFileRelation, cancellationToken);
            }

            var notification = new GeoFileActivityOccuredNotification {
                GeoFileId = geoFile.Id,
                ActivityType = ActivityType.Shared
            };
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}