using System;
using Autofac;
using Microsoft.AspNetCore.Mvc;
using SimpleMapViewer.Infrastructure.Constants;

namespace SimpleMapViewer.Backend.Application.Common {
    [ApiController]
    [Route("api/v1/[controller]s")]
    [Consumes("application/json")]
    public class ApiBaseController : ControllerBase {
        private readonly Func<object, ILifetimeScope> _taggedLifetimeScopeFactory;

        public ApiBaseController(Func<object, ILifetimeScope> taggedLifetimeScopeFactory) {
            _taggedLifetimeScopeFactory = taggedLifetimeScopeFactory;
        }

        [NonAction]
        protected ILifetimeScope OpenUnitOfWorkScoop() {
            return _taggedLifetimeScopeFactory(LifetimeScopeTags.UNIT_OF_WORK);
        }
    }
}