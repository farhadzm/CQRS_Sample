using AutoMapper;
using System;
using System.Linq;
using System.Reflection;

namespace CQRS.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }
        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(t => t.GetInterfaces().Any(i =>
                    i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) || i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                MethodInfo methodInfo = null;
                if (type.GetInterfaces().Any(a => a.IsGenericType && a.GetGenericTypeDefinition() == typeof(IMapFrom<>)))
                    methodInfo = type?.GetMethod("Mapping")
                            ?? type?.GetInterface("IMapFrom`1").GetMethod("Mapping");
                else
                    methodInfo = type?.GetMethod("Mapping")
                     ?? type?.GetInterface("IMapTo`1").GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
