using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.Dtos;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;
using ISession = NHibernate.ISession;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Queries.GetAllGeoFiles {
    internal class GetAllGeoFilesHandler :
        IRequestHandler<GetAllGeoFilesRequest, GetAllGeoFilesResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetAllGeoFilesHandler(
            IUnitOfWork<ISession> unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor
        ) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetAllGeoFilesResponse> Handle(
            GetAllGeoFilesRequest request,
            CancellationToken cancellationToken
        ) {
            var authUser = _httpContextAccessor.HttpContext.User.ToAuthUser();

            var query = _session.Query<Domain.Entities.GeoFile>()
                .Join(
                    _session.Query<UserGeoFileRelation>(),
                    file => file, relation => relation.GeoFile,
                    (file, relation) => new {GeoFile = file, Relation = relation}
                )
                .Where(x => x.Relation.User.Id == authUser.Id);
            query = request.Shared
                ? query.Where(x =>
                    x.Relation.AccessType == AccessType.Watch
                    || x.Relation.AccessType == AccessType.Comment
                    || x.Relation.AccessType == AccessType.Edit
                )
                : query.Where(x => x.Relation.AccessType == AccessType.Own);

            var geoFiles = await query.Select(x => x.GeoFile).ToListAsync(cancellationToken);
            var geoFileDtos = _mapper.Map<IList<GeoFileOutDto>>(geoFiles);
            foreach (var geoFileDto in geoFileDtos) {
                var user = await _session.Query<UserGeoFileRelation>()
                    .Where(x => x.GeoFile.Id == geoFileDto.Id && x.AccessType == AccessType.Own)
                    .Select(x => x.User)
                    .SingleOrDefaultAsync(cancellationToken);
                geoFileDto.Owner = user?.Name;
            }

            return new GetAllGeoFilesResponse {GeoFiles = geoFileDtos};
        }
    }
}