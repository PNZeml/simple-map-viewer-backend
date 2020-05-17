﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Features.User.Dtos;

namespace SimpleMapViewer.Backend.Application.Features.User.Queries.GetAllUsersBy {
   internal class GetAllUsersByRequestHandler :
       IRequestHandler<GetAllUsersByRequest, GetAllUsersByResponse> {
       private readonly ISession _session;
       private readonly IMapper _mapper;

       public GetAllUsersByRequestHandler(IUnitOfWork<ISession> unitOfWork, IMapper mapper) {
           _session = unitOfWork.Source;
           _mapper = mapper;
       }

        public async Task<GetAllUsersByResponse> Handle(
            GetAllUsersByRequest request,
            CancellationToken cancellationToken
        ) {
            var searchQuery = request.SearchQuery.ToLower().Trim();
            var users = await _session.Query<Domain.Entities.User>()
                .Where(x =>
                    x.Name.ToLower().Contains(searchQuery)
                    || x.Email.ToLower().Contains(searchQuery)
                )
                .Take(100)
                .ToListAsync(cancellationToken);
            var userDtos = _mapper.Map<IList<UserDto>>(users);
            return new GetAllUsersByResponse { Users = userDtos };
        }
    }
}