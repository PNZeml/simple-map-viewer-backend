using System;
using Autofac;
using Microsoft.AspNetCore.SignalR;
using SimpleMapViewer.Infrastructure.Constants;

namespace SimpleMapViewer.Backend.Application.Common {
    internal abstract class HubBaseController<THub, TClientRouter>
        where THub : Hub<TClientRouter>
        where TClientRouter : class {
        protected IHubContext<THub, TClientRouter> Context { get; }
        
        private readonly Func<object, ILifetimeScope> _taggedLifetimeScopeFactory;

        public HubBaseController(
            IHubContext<THub, TClientRouter> context,
            Func<object, ILifetimeScope> taggedLifetimeScopeFactory
        ) {
            Context = context;
            _taggedLifetimeScopeFactory = taggedLifetimeScopeFactory;
        }
        
        protected ILifetimeScope OpenUnitOfWorkScoop() =>
            _taggedLifetimeScopeFactory(LifetimeScopeTags.UNIT_OF_WORK);
    }
}