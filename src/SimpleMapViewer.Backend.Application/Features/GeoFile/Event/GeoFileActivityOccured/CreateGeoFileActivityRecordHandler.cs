using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Event.GeoFileActivityOccured {
    internal class CreateGeoFileActivityRecordHandler :
        INotificationHandler<GeoFileActivityOccuredNotification> {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public CreateGeoFileActivityRecordHandler(
            IUnitOfWork<ISession> unitOfWork,
            IMapper mapper
        ) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task Handle(
            GeoFileActivityOccuredNotification notification,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                notification.GeoFileId,
                cancellationToken
            );
            var user = await _session.GetAsync<Domain.Entities.User>(
                notification.UserId,
                cancellationToken
            );

            var record = new GeoFileActivityRecord {
                GeoFile = geoFile,
                User = user,
                ActivityType = notification.ActivityType,
                Occured = DateTime.Now
            };
            await _session.SaveAsync(record);
        }
    }
}