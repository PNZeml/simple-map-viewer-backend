using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Share {
    internal class ShareHandler : AsyncRequestHandler<ShareRequest> {
        private readonly ISession _session;
        private readonly IMediator _mediator;
        
        public ShareHandler(IUnitOfWork<ISession> unitOfWork, IMediator mediator) {
            _session = unitOfWork.Source;
            _mediator = mediator;
        }
        
        protected override async Task Handle(
            ShareRequest request,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                request.GeoFileId,
                cancellationToken
            );
            foreach (var userId in request.UserIds) {
                if (cancellationToken.IsCancellationRequested) break;
    
                var user = await _session.GetAsync<Domain.Entities.User>(
                    userId,
                    cancellationToken
                );
                var relation = await _session.Query<UserGeoFilesRelation>()
                    .FirstOrDefaultAsync(
                        x => x.User == user && x.GeoFile == geoFile,
                        cancellationToken
                    );
                if (relation != null) {
                    relation.AccessType = request.AccessType;
                    continue;
                }

                relation = new UserGeoFilesRelation {
                    GeoFile = geoFile,
                    User = user,
                    AccessType = request.AccessType
                };
                await _session.SaveAsync(relation, cancellationToken);
            }

            var notification = new GeoFileActivityOccuredNotification {
                GeoFileId = geoFile.Id,
                UserId = request.UserId,
                ActivityType = ActivityType.Shared
            };
            await _mediator.Publish(notification, cancellationToken);
        }
    }
}