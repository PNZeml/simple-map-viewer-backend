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

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAll {
    internal class GetAllHandler : IRequestHandler<GetAllRequest, GetAllResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public GetAllHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task<GetAllResponse> Handle(
            GetAllRequest request,
            CancellationToken cancellationToken
        ) {
            var query = _session.Query<Domain.Entities.GeoFile>()
                .Join(
                    _session.Query<UserGeoFilesRelation>(),
                    file => file, relation => relation.GeoFile,
                    (file, relation) => new {GeoFile = file, Relation = relation}
                )
                .Where(x => x.Relation.User.Id == request.UserId);
            if (request.Shared) {
                query = query.Where(x =>
                    x.Relation.AccessType == AccessType.Watch
                    || x.Relation.AccessType == AccessType.Comment
                    || x.Relation.AccessType == AccessType.Edit
                );
            } else {
                query = query.Where(x => x.Relation.AccessType == AccessType.Own);
            }
            var geoFiles = await query.Select(x => x.GeoFile).ToListAsync(cancellationToken);
            var geoFileDtos = _mapper.Map<IList<GeoFileDto>>(geoFiles);
            return new GetAllResponse {GeoFiles = geoFileDtos};
        }
    }
}