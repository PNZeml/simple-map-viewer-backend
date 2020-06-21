using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.User.Queries.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsers {
   internal class GetAllUsersHandler : IRequestHandler<GetAllUsersRequest, GetAllUsersResponse> {
       private readonly ISession _session;
       private readonly IMapper _mapper;

       public GetAllUsersHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
           _session = unitOfWork.Source;
           _mapper = mapper;
       }

        public async Task<GetAllUsersResponse> Handle(
            GetAllUsersRequest request,
            CancellationToken cancellationToken
        ) {
            var usersQuery = _session.Query<Domain.Entities.User>();
            if (request.SearchString != null) {
                var searchQuery = request.SearchString?.ToLower().Trim();
                usersQuery = usersQuery
                    .Where(x =>
                        x.Name.ToLower().Contains(searchQuery)
                        || x.Email.ToLower().Contains(searchQuery)
                    );
            }
            var users = await usersQuery.Take(100).ToListAsync(cancellationToken);
            var userDtos = _mapper.Map<IList<UserOutDto>>(users);
            return new GetAllUsersResponse { Users = userDtos };
        }
    }
}