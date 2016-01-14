using Domain;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;

namespace DataAccess.AutoMappingOverrides
{
    public class PathMappingOverride : IAutoMappingOverride<Path>
    {
        public void Override(AutoMapping<Path> mapping)
        {
            mapping
                .Map(p => p.Components)
                .CustomType<PathComponentsList>()
                .Column(string.Empty)
                .Unique();
        }
    }
}
