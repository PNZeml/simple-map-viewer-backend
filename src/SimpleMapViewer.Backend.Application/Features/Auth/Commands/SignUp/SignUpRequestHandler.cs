using System;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common.AvatarGenerator;

namespace SimpleMapViewer.Backend.Application.Features.Auth.Commands.SignUp {
    internal class SignUpRequestHandler : AsyncRequestHandler<SignUpRequest> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly AvatarGeneratorBuilder _avatarGeneratorBuilder;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
        
        public SignUpRequestHandler(
            IUnitOfWork<ISession> unitOfWork,
            IMapper mapper,
            AvatarGeneratorBuilder avatarGeneratorBuilder
        ) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _avatarGeneratorBuilder = avatarGeneratorBuilder;
            _passwordHasher = new PasswordHasher<Domain.Entities.User>();
        }

        protected override async Task Handle(
            SignUpRequest request,
            CancellationToken cancellationToken
        ) {
            var user = _mapper.Map<Domain.Entities.User>(request);
            user.PasswordHashed = _passwordHasher.HashPassword(user, request.Password);
            user.AccessToken = Guid.NewGuid();
            user.Avatar = _avatarGeneratorBuilder.SetPadding(1).ToBytes(ImageFormat.Jpeg);

            await _session.SaveAsync(user, cancellationToken);
        }
    }
}