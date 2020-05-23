using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllByGeoFile {
    internal class GetAllByGeoFileHandler :
        IRequestHandler<GetAllByGeoFileRequest, GetAllByGeoFileResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public GetAllByGeoFileHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task<GetAllByGeoFileResponse> Handle(
            GetAllByGeoFileRequest request,
            CancellationToken cancellationToken
        ) {
            var geoFile = await _session.GetAsync<Domain.Entities.GeoFile>(
                request.GeoFileId,
                cancellationToken
            );

            var userAccessTypes = await _session.Query<Domain.Entities.User>()
                .Join(
                    _session.Query<UserGeoFilesRelation>(),
                    user => user, relation => relation.User,
                    (user, relation) => new {User = user, Relation = relation}
                )
                .Where(x => x.Relation.GeoFile == geoFile)
                .Select(x => new {User = x.User, AccessType = x.Relation.AccessType})
                .ToListAsync(cancellationToken);
            var userAccessTypeDtos = userAccessTypes
                .Select(x => {
                    var userAccessTypeDto = _mapper.Map<UserAccessTypeDto>(x.User);
                    userAccessTypeDto.AccessType = x.AccessType;
                    return userAccessTypeDto;
                })
                .ToList();
            
            return new GetAllByGeoFileResponse { Users = userAccessTypeDtos };
        }
    }
}