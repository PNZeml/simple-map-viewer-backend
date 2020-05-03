using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Queries.SignIn {
    internal class SignInHandler : IRequestHandler<SignInRequest, SignInResponse> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        
        public SignInHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
        }
        
        // TODO: Process exceptions
        public async Task<SignInResponse> Handle(
            SignInRequest request,
            CancellationToken cancellationToken
        ) {
            var user = await _session.Query<User>()
                .FirstOrDefaultAsync(
                    x => x.Name == request.Login || x.Email == request.Login,
                    cancellationToken
                );
            if (user == null) {
                throw new ArgumentNullException();
            }

            var verificationResult =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, request.Password);
            if (verificationResult == PasswordVerificationResult.Failed) {
                throw new ArgumentException();
            }

            return _mapper.Map<SignInResponse>(user);
        }
    }
}