using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllBy {
    internal class GetAllByHandler : IRequestHandler<GetAllByRequest, GetAllByResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public GetAllByHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task<GetAllByResponse> Handle(
            GetAllByRequest request,
            CancellationToken cancellationToken
        ) {
            var query = _session.Query<Domain.Entities.GeoFile>()
                .Join(
                    _session.Query<UserGeoFilesRelation>(),
                    file => file, relation => relation.GeoFile,
                    (file, relation) => new {GeoFile = file, Relation = relation}
                )
                .Where(x => x.Relation.User.Id == request.UserId);
            if (request.IsSharedRequested) {
                query = query.Where(x =>
                    x.Relation.AccessType == AccessType.Watch
                    || x.Relation.AccessType == AccessType.Comment
                    || x.Relation.AccessType == AccessType.Edit
                );
            } else {
                query = query.Where(x => x.Relation.AccessType == AccessType.Owner);
            }
            var geoFiles = await query.Select(x => x.GeoFile).ToListAsync(cancellationToken);
            var geoFileDtos = _mapper.Map<IList<GeoFileDto>>(geoFiles);
            return new GetAllByResponse {GeoFiles = geoFileDtos};
        }
    }
}