using System.Collections.Generic;
using System.Linq;
using Mapster;

namespace Dto.Conversions
{
    public static class AutoMappingConfiguration
    {
        public static void Run()
        {
            TypeAdapterConfig<IDictionary<string, object>, IDictionary<string, object>>
                .NewConfig()
                .MapWith(dictionary => dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
        }
    }
}
