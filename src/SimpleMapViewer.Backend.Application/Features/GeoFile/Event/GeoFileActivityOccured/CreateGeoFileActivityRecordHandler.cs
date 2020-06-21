using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using SimpleMapViewer.Domain.Entities;
using ISession = NHibernate.ISession;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured {
    internal class CreateGeoFileActivityRecordHandler :
        INotificationHandler<GeoFileActivityOccuredNotification> {
        private readonly ISession _session;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateGeoFileActivityRecordHandler(
            IUnitOfWork<ISession> unitOfWork,
            IHttpContextAccessor httpContextAccessor
        ) {
            _session = unitOfWork.Source;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Handle(
            GeoFileActivityOccuredNotification notification,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                notification.GeoFileId,
                cancellationToken
            );
            var authUser = _httpContextAccessor.HttpContext.User.ToAuthUser();
            var user = await _session.GetAsync<Domain.Entities.User>(
                authUser.Id,
                cancellationToken
            );
            var geoFileActivityRecord = new GeoFileActivityRecord {
                GeoFile = geoFile,
                User = user,
                ActivityType = notification.ActivityType,
                Occurred = DateTime.Now
            };
            await _session.SaveAsync(geoFileActivityRecord, cancellationToken);
        }
    }
}