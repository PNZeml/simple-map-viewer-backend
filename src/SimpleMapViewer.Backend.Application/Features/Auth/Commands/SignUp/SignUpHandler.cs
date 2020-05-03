using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp {
    internal class SignUpHandler : AsyncRequestHandler<SignUpRequest> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        
        public SignUpHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<User>();
        }

        protected override async Task Handle(
            SignUpRequest request,
            CancellationToken cancellationToken
        ) {
            var user = _mapper.Map<User>(request);
            user.PasswordHashed = _passwordHasher.HashPassword(user, request.Password);
            user.AccessToken = Guid.NewGuid();

            await _session.SaveAsync(user, cancellationToken);
        }
    }
}