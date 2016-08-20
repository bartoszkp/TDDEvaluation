using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class MissingValuePolicyBasic : TestsBase
    {
        [ClassInitialize]
        public static new void ClassInitialize(TestContext testContext)
        {
            TestsBase.ClassInitialize(testContext);
        }

        [ClassCleanup]
        public static new void ClassCleanup()
        {
            TestsBase.ClassCleanup();
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenANewSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            ForAllSignalTypes((dataType, granularity)
               =>
            {
                GivenASignalWith(dataType, granularity);

                ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
            });
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenNonexistingSignalId_WhenGettingMissingValuePolicy_ServiceThrows()
        {
            Assertions.AssertThrows(() => client.GetMissingValuePolicy(0));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenNonexistingSignalId_WhenSettingsMissingValuePolicy_ServiceThrows()
        {
            var mvp = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>()
                .ToDto<Dto.MissingValuePolicy.MissingValuePolicy>();

            Assertions.AssertThrows(() => client.SetMissingValuePolicy(0, mvp));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            ForAllSignalTypes((dataType, granularity)
                =>
            {
                GivenASignalWith(dataType, granularity);
                WithNonDefaultMissingValuePolicyFor(dataType);

                WhenSettingMissingValuePolicy(
                    typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                    dataType);

                ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
            });
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASignal_WhenSettingSpecificValueMissingValuePolicy_PolicyIsChangedForSignal()
        {
            ForAllSignalTypesAndQualites((dataType, granularity, quality)
                =>
            {
                GivenASignalWith(dataType, granularity);

                WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

                ThenSignalHasSpecificMissingValuePolicyWith(quality);
            });
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            ForAllSignalTypes((dataType, granularity)
                =>
            {
                GivenASignalWith(dataType, granularity);

                WhenSettingMissingValuePolicy(
                    typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                    dataType);

                ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
            });
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            ForAllSignalTypes((dataType, granularity)
                =>
            {
                if (dataType == DataType.String)
                    return;

                GivenASignalWith(dataType, granularity);

                WhenSettingMissingValuePolicy(
                    typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                    dataType);

                ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
            });
        }

        private static Domain.MissingValuePolicy.MissingValuePolicyBase CreateForNativeType(
            Type genericPolicyType, Type nativeType)
        {
            return genericPolicyType
                .MakeGenericType(nativeType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null) as Domain.MissingValuePolicy.MissingValuePolicyBase;
        }

        private void WhenSettingSpecificValueMissingValuePolicy(DataType dataType, Quality quality)
        {
            var newPolicy = CreateForNativeType(
               typeof(Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<>),
               Domain.Infrastructure.DataTypeUtils.GetNativeType(dataType));

            var dtoPolicy = newPolicy.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();
            dtoPolicy.Quality = quality.ToDto<Dto.Quality>();

            client.SetMissingValuePolicy(signalId, dtoPolicy);
        }

        private void WhenSettingMissingValuePolicy(Type policyType, DataType signalDataType)
        {
            var newPolicy = CreateForNativeType(
                policyType,
                Domain.Infrastructure.DataTypeUtils.GetNativeType(signalDataType));

            client.SetMissingValuePolicy(signalId, newPolicy.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
        }

        private void WithNonDefaultMissingValuePolicyFor(DataType dataType)
        {
            WhenSettingMissingValuePolicy(typeof(Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<>), dataType);
        }

        private void ThenMissingValuePolicyHasType(Type type)
        {
            Assert.IsInstanceOfType(client.GetMissingValuePolicy(signalId), type);
        }

        private void ThenSignalHasSpecificMissingValuePolicyWith(Quality quality)
        {
            var policy = (Dto.MissingValuePolicy.SpecificValueMissingValuePolicy)client.GetMissingValuePolicy(signalId);
            Assert.AreEqual(quality.ToDto<Dto.Quality>(), policy.Quality);
        }
    }
}
