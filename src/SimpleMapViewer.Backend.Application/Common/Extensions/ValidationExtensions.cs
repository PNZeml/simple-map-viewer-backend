using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Http;
using NHibernate.Linq;
using Shura.Data;
using SimpleMapViewer.Domain.Entities;
using ISession = NHibernate.ISession;

namespace SimpleMapViewer.Backend.Application.Common.Extensions {
    internal static class ValidationExtensions {
        public static IDictionary<string, string[]> FormatValidationErrors(
            this IEnumerable<ValidationFailure> validationFailures
        ) {
            return validationFailures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(x => x.Key, group => group.Select(x => x.ErrorMessage).ToArray());
        }

        public static IRuleBuilderInitial<T, long> UserHasAccessToGeoFile<T>(
            this IRuleBuilder<T, long> ruleBuilder,
            ILifetimeScope lifetimeScope,
            IHttpContextAccessor httpContextAccessor
        ) {
            async Task UserHasAccessToGeoFile(
                long geoFileId,
                CustomContext context,
                CancellationToken cancellationToken
            ) {
                var authUser = httpContextAccessor.HttpContext.User.ToAuthUser();

                var unitOfWork = lifetimeScope.Resolve<IUnitOfWork<ISession>>();
                var session = unitOfWork.Source;

                var userGeoFilesRelation = await session.Query<UserGeoFileRelation>()
                    .Where(x => x.GeoFile.Id == geoFileId)
                    .Where(x => x.User.Id == authUser.Id)
                    .SingleOrDefaultAsync(cancellationToken);
                if (userGeoFilesRelation == null) {
                    context.AddFailure(
                        $"User does not have access to GeoFile with id {geoFileId}"
                    );
                }
            }

            return ruleBuilder
                .CustomAsync(UserHasAccessToGeoFile);
        }
    }
}