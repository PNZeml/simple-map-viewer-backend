using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using Shura.Data;
using SimpleMapViewer.Backend.Application.Common;
using SimpleMapViewer.Backend.Application.Common.Extensions;
using ISession = NHibernate.ISession;

namespace SimpleMapViewer.Backend.Application.Features.GeoFile.Commands.ShareGeoFile {
    internal class ShareGeoFileRequestValidator : RequestBaseValidator<ShareGeoFileRequest> {
        public ShareGeoFileRequestValidator(
            IHttpContextAccessor httpContextAccessor,
            ILifetimeScope lifetimeScope
        ) : base(lifetimeScope) {
            RuleFor(x => x.GeoFileId)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MustAsync(GeoFileExists)
                .WithMessage("GeoFile dose not exist")
                .UserHasAccessToGeoFile(lifetimeScope, httpContextAccessor);
        }
        
        private async Task<bool> GeoFileExists(
            ShareGeoFileRequest request,
            long geoFileId,
            PropertyValidatorContext context,
            CancellationToken cancellationToken
        ) {
            var unitOfWork = LifetimeScope.Resolve<IUnitOfWork<ISession>>();
            var session = unitOfWork.Source;
 
            var geoFile = await session.GetAsync<Domain.Entities.GeoFile>(
                geoFileId,
                cancellationToken
            );
            return geoFile != null;
        }
    }
}