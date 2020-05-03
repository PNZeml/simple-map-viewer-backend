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
            var geoFiles = await _session.Query<Domain.Entities.GeoFile>()
                /*.Where(x => x.OwnerUser.Id == request.UserId)*/
                .ToListAsync(cancellationToken);
            var geoFileDtos = _mapper.Map<IList<GeoFileDto>>(geoFiles);
            return new GetAllResponse {GeoFiles = geoFileDtos};
        }
    }
}