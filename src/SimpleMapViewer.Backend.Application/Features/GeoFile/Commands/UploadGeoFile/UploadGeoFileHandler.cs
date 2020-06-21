using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;
using ISession = NHibernate.ISession;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.UploadGeoFile {
    internal class UploadGeoFileHandler : AsyncRequestHandler<UploadGeoFileRequest> {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly ISession _session;
        
        public UploadGeoFileHandler(
            IUnitOfWork<ISession> unitOfWork,
            IHttpContextAccessor httpContextAccessor,
            IMediator mediator
        ) {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _session = unitOfWork.Source;
        }

        protected override async Task Handle(
            UploadGeoFileRequest geoFileRequest,
            CancellationToken cancellationToken
        ) {
            var geoFile = new Domain.Entities.GeoFile {
                Name = "new_york.geojson",
                Size = 1565846,
                Created = DateTime.Now
            };
            await _session.SaveAsync(geoFile, cancellationToken);

            var authUser = _httpContextAccessor.HttpContext.User.ToAuthUser();
            
            var user = await _session.GetAsync<Domain.Entities.User>(authUser.Id, cancellationToken);
            await _session.SaveAsync(
                new UserGeoFileRelation { 
                    AccessType = AccessType.Own,
                    GeoFile = geoFile,
                    User = user
                },
                cancellationToken
            );

            var activityNotification = new GeoFileActivityOccuredNotification {
                ActivityType = ActivityType.Created, GeoFileId = geoFile.Id
            };
            await _mediator.Publish(activityNotification, cancellationToken);
        }
    }
}