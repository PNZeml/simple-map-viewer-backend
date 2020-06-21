using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using NHibernate;
using NHibernate.Linq;
using Nito.AsyncEx;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Features.Map.Dtos;
using SimpleMapViewer.Backend.Application.Features.Map.Hub;
using SimpleMapViewer.Domain.AppEntities;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Backend.Application.Features.Map.Controller {
    internal class MapHubController :
        HubBaseController<MapHub, IMapClientRouter>,
        IMapHubController {
        private readonly IMapper _mapper;
        private readonly HashSet<GeoFileGroup> _geoFileGroups;
        private readonly AsyncReaderWriterLock _lock;

        public MapHubController(
            IMapper mapper,
            IHubContext<MapHub, IMapClientRouter> context,
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) : base(context, taggedLifetimeScopeFactory) {
            _mapper = mapper;
            _geoFileGroups = new HashSet<GeoFileGroup>();
            _lock = new AsyncReaderWriterLock();
        }

        public async Task AddUser(UserInDto user, long geoFileId) {
            using var @lock = await _lock.WriterLockAsync();

            var geoFileGroup =
                _geoFileGroups.SingleOrDefault(x => x.GeoFileId == geoFileId)
                ?? new GeoFileGroup(geoFileId);

            _mapper.Map(await GetUser(user.Id), user);
            
            geoFileGroup.Users.Add(user);
            _geoFileGroups.Add(geoFileGroup);
            await Context.Groups.AddToGroupAsync(user.ConnectionId, geoFileGroup.ToString());

            await NotifyUsersUpdated(geoFileGroup);
            await NotifyCommentsUpdated(geoFileGroup);
        }

        public async Task RemoveUser(string connectionId) {
            using var @lock = await _lock.WriterLockAsync();

            var geoFileGroup = _geoFileGroups.SingleOrDefault(x =>
                x.Users.Select(user => user.ConnectionId).Contains(connectionId)
            );

            if (geoFileGroup == null) return;

            geoFileGroup.Users.RemoveWhere(x => x.ConnectionId == connectionId);
            await Context.Groups.RemoveFromGroupAsync(connectionId, geoFileGroup.ToString());
            await NotifyUsersUpdated(geoFileGroup);
            
            if (geoFileGroup.Users.Count == 0) _geoFileGroups.Remove(geoFileGroup);
        }

        public async Task UpdateUserViewport(
            string connectionId,
            double x,
            double y,
            double height,
            double width
        ) {
            using var @lock = await _lock.WriterLockAsync();

            var geoFileGroup = _geoFileGroups.SingleOrDefault(x =>
                x.Users.Select(user => user.ConnectionId).Contains(connectionId)
            );

            var user = geoFileGroup?.Users.SingleOrDefault(x => x.ConnectionId == connectionId);
            if (user == null) return;

            user.X = x;
            user.Y = y;
            user.Height = height;
            user.Width = width;

            await NotifyUsersUpdated(geoFileGroup);
        }

        public async Task CreateComment(AuthUser authUser, double x, double y, string comment) {
            using var @lock = await _lock.ReaderLockAsync();

            var geoFileGroup = _geoFileGroups.SingleOrDefault(x =>
                x.Users.Select(x => x.Id).Contains(authUser.Id)
            );
            
            if (geoFileGroup == null) return;

            await using var unitOfWorkLifetimeScoop = OpenUnitOfWorkScoop();
            var unitOfWork = unitOfWorkLifetimeScoop.Resolve<IUnitOfWork<ISession>>();
            unitOfWork.Begin();

            var session = unitOfWork.Source;

            var user = await session.GetAsync<Domain.Entities.User>(authUser.Id);
            var geoFile = await session.GetAsync<Domain.Entities.GeoFile>(geoFileGroup.GeoFileId);
            
            var geoFileComment = new GeoFileComment {
                X = Convert.ToDecimal(x),
                Y = Convert.ToDecimal(y),
                Comment = comment,
                User = user,
                GeoFile = geoFile
            };
            await session.SaveAsync(geoFileComment);

            var geoFileActivityRecord = new GeoFileActivityRecord {
                ActivityType = ActivityType.Commented,
                User = user,
                GeoFile = geoFile,
                Occurred = DateTime.Now
            };
            await session.SaveAsync(geoFileActivityRecord);

            await unitOfWork.CommitAsync();

            await NotifyCommentsUpdated(geoFileGroup);
        }

        private async Task<Domain.Entities.User> GetUser(long userId) {
            await using var unitOfWorkLifetimeScoop = OpenUnitOfWorkScoop();
            var unitOfWork = unitOfWorkLifetimeScoop.Resolve<IUnitOfWork<ISession>>();
            unitOfWork.Begin();

            var session = unitOfWork.Source;
            var user = await session.GetAsync<Domain.Entities.User>(userId);

            await unitOfWork.CommitAsync();

            return user;
        }

        private Task NotifyUsersUpdated(GeoFileGroup geoFileGroup) {
            var userOutDtos = _mapper.Map<IList<UserOutDto>>(geoFileGroup.Users);
            return Context.Clients.Group(geoFileGroup.ToString()).UpdateUsers(userOutDtos);
        }

        private async Task NotifyCommentsUpdated(GeoFileGroup geoFileGroup) {
            await using var unitOfWorkLifetimeScoop = OpenUnitOfWorkScoop();
            var unitOfWork = unitOfWorkLifetimeScoop.Resolve<IUnitOfWork<ISession>>();
            unitOfWork.Begin();

            var session = unitOfWork.Source;
            var geoFileComments = await session.Query<GeoFileComment>()
                .Where(x => x.GeoFile.Id == geoFileGroup.GeoFileId)
                .ToListAsync();
           
            await unitOfWork.CommitAsync();

            var geoFileCommentsDtos = _mapper.Map<IList<GeoFileCommentOutDto>>(geoFileComments);
            await Context.Clients.Group(geoFileGroup.ToString()).UpdateComments(geoFileCommentsDtos);
        }
    }
}