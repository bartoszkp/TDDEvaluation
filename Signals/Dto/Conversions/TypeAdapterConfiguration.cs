using System.Linq;

namespace Dto.Conversions
{
    public static class TypeAdapterConfiguration
    {
        public static void Initialize()
        {
            var datumTypes = typeof(DataAccess.IUnitOfWorkProvider)
                .Assembly
                .GetTypes()
                .Where(t => t.BaseType != null)
                .Where(t => t.BaseType.IsGenericType)
                .Where(t => t.BaseType.GetGenericTypeDefinition().Equals(typeof(Domain.Datum<>)));

            var configureDatumMapping = typeof(TypeAdapterConfiguration)
                    .GetMethod("ConfigureDatumMapping", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            foreach (var datumType in datumTypes)
            {
                var nativeType = datumType.BaseType.GenericTypeArguments[0];

                configureDatumMapping
                    .MakeGenericMethod(nativeType)
                    .Invoke(null, null);
            }
        }

        private static void ConfigureDatumMapping<T>()
        {
            FastMapper
                .TypeAdapterConfig<Domain.Datum<T>, Dto.Datum>
                .NewConfig()
                .MapFrom(target => target.Value, source => source.Value)
                .MapFrom(target => target.Timestamp, source => source.Timestamp);

            FastMapper
                .TypeAdapterConfig<Dto.Datum, Domain.Datum<T>>
                .NewConfig()
                .MapFrom(target => target.Value, source => source.Value)
                .MapFrom(target => target.Timestamp, source => source.Timestamp);
        }
    }
}
