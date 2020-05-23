using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Upload {
    internal class UploadHandler : AsyncRequestHandler<UploadRequest> {
        private readonly ISession _session;
        
        public UploadHandler(IUnitOfWork<ISession> unitOfWork) {
            _session = unitOfWork.Source;
        }

        protected override async Task Handle(
            UploadRequest request,
            CancellationToken cancellationToken
        ) {
            var geoFile = new Domain.Entities.GeoFile {
                Name = "test2.geojson",
                Size = 1024,
                Created = DateTime.Now
            };
            await _session.SaveAsync(geoFile, cancellationToken);
            var user = await _session.GetAsync<Domain.Entities.User>(5L, cancellationToken);
            await _session.SaveAsync(
                new UserGeoFilesRelation { 
                    AccessType = AccessType.Own,
                    GeoFile = geoFile,
                    User = user
                },
                cancellationToken
            );
        }
    }
}