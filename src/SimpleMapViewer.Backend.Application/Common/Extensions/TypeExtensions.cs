using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SimpleMapViewer.Backend.Application.Common.Extensions {
    internal static class TypeExtensions {
        public static IEnumerable<Type> GetTypesOfInterface(
            this Assembly assembly,
            Type interfaceType
        ) => assembly.GetTypes().Where(x => x.DoesImplementInterfaces(interfaceType));

        public static bool DoesImplementInterfaces(
            this Type implementationType,
            params Type[] interfaceTypes
        ) =>
            implementationType
                .GetInterfaces()
                .Any(x =>
                    x.IsGenericType && interfaceTypes.Contains(x.GetGenericTypeDefinition())
                    || interfaceTypes.Contains(x)
                );
    }
}