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

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetGeoFile {
    internal class GetGeoFileHandler : IRequestHandler<GetGeoFileRequest, GetGeoFileResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        
        public GetGeoFileHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task<GetGeoFileResponse> Handle(
            GetGeoFileRequest request,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                request.GeoFileId,
                cancellationToken
            );
            var geoFileOutDto = _mapper.Map<GeoFileOutDto>(geoFile);

            var users = await _session.Query<Domain.Entities.User>()
                .Join(
                    _session.Query<UserGeoFileRelation>(),
                    user => user, relation => relation.User,
                    (user, relation) => new {User = user, Relation = relation}
                )
                .Where(x => x.Relation.GeoFile == geoFile)
                .Select(x => new { x.User, x.Relation.AccessType })
                .ToListAsync(cancellationToken);
            var userOutDtos = users
                .Select(x => {
                    var user = _mapper.Map<UserOutDto>(x.User);
                    user.AccessType = x.AccessType;
                    return user;
                })
                .ToList();
            geoFileOutDto.Users = userOutDtos;

            var geoFileActivityRecords = await _session.Query<GeoFileActivityRecord>()
                .Where(x => x.GeoFile.Id == geoFile.Id)
                .ToListAsync(cancellationToken);
            var geoFileActivityRecordDtos =
                _mapper.Map<IList<GeoFileActivityRecordOutDto>>(geoFileActivityRecords);
            geoFileOutDto.GeoFileActivityRecords = geoFileActivityRecordDtos;

            var user = await _session.Query<UserGeoFileRelation>()
                .Where(x => x.GeoFile.Id == geoFileOutDto.Id && x.AccessType == AccessType.Own)
                .Select(x => x.User)
                .SingleOrDefaultAsync(cancellationToken);
            geoFileOutDto.Owner = user?.Name;

            return new GetGeoFileResponse { GeoFile = geoFileOutDto };
        }
    }
}