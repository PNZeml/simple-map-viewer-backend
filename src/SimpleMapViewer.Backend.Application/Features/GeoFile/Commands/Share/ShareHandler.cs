using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NHibernate;
using Shura.Data;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.Share {
    internal class ShareHandler : AsyncRequestHandler<ShareRequest> {
        private readonly ISession _session;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;
        
        public ShareHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
            _session = unitOfWork.Source;
            _mapper = mapper;
            _passwordHasher = new PasswordHasher<Domain.Entities.User>();
        }
        
        protected override async Task Handle(
            ShareRequest request,
            CancellationToken cancellationToken
        ) {
            
        }
    }
}