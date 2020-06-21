using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.GetUserByAccessToken {
    internal class GetUserByAccessTokenRequestHandler :
        IRequestHandler<GetUserByAccessTokenRequest, GetUserByAccessTokenResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;

        public GetUserByAccessTokenRequestHandler(
            IUnitOfWork<ISession> unitOfWork,
            IMapper mapper
        ) {
            _session = unitOfWork.Source;
            _mapper = mapper;
        }

        public async Task<GetUserByAccessTokenResponse> Handle(
            GetUserByAccessTokenRequest request,
            CancellationToken cancellationToken
        ) {
            var user = await _session.Query<Domain.Entities.User>()
                .FirstOrDefaultAsync(x =>
                    x.AccessToken.ToString() == request.AccessToken.ToString(),
                    cancellationToken
                );

            UserOutDto userOutDto = null;
            if (user != null) {
                userOutDto = _mapper.Map<UserOutDto>(user);
            }

            return new GetUserByAccessTokenResponse { UserOut = userOutDto };
        }
    }
}