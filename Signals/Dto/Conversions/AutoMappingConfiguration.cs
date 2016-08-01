using System;
using System.Linq;
using System.Linq.Expressions;
using Domain.Infrastructure;
using Mapster;

namespace Dto.Conversions
{
    public static class AutoMappingConfiguration
    {
        public static void Run()
        {
            TypeAdapterConfig.GlobalSettings.When((s, t, m) => t.Namespace.Equals(typeof(MissingValuePolicy.MissingValuePolicy).Namespace))
                .Settings
                .AfterMappingFactories
                .Add(SetDataTypeIfNeededFactory);
        }

        private static LambdaExpression SetDataTypeIfNeededFactory(CompileArgument ca)
        {
            Expression<Action<object, object>> factory = ((object source, object result) => SetDataTypeIfNeeded(source, result));
            return factory;
        }

        private static void SetDataTypeIfNeeded(object source, object result)
        {
            if (!source.GetType().IsGenericType
                || source.GetType().GetGenericArguments().Length != 1)
            {
                return;
            }

            var dataTypeProperty = MapFromGenericDataTypeAttribute.GetSinglePropertyMappedFromGenericDataTypeOrNull(result.GetType());

            if (dataTypeProperty == null)
            {
                return;
            }

            var dataTypeValue = DataTypeUtils.FromNativeType(source.GetType().GetGenericArguments().Single()).ToDto<Dto.DataType>();

            dataTypeProperty.SetValue(result, dataTypeValue);
        }
    }
}
