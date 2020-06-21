using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.Auth.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn {
    internal class SignInHandler : IRequestHandler<SignInRequest, SignInResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
        
        public SignInHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<Domain.Entities.User>();
        }

        public async Task<SignInResponse> Handle(
            SignInRequest request,
            CancellationToken cancellationToken
        ) {
            var user = await _session.Query<Domain.Entities.User>()
                .FirstOrDefaultAsync(
                    x => x.Name == request.Login || x.Email == request.Login,
                    cancellationToken
                );
            if (user == null) {
                return new SignInResponse { User = null };
            }

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed) {
                return new SignInResponse { User = null };
            }

            var userDto = _mapper.Map<UserOutDto>(user);
            return new SignInResponse { User = userDto };
        }
    }
}