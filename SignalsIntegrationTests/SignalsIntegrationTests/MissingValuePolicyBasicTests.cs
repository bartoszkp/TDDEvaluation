using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class MissingValuePolicyBasic: TestsBase
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
        public void NewSignalHasNoneQualityMissingValuePolicy()
        {
            var signal = AddNewIntegerSignal();

            var result = client.GetMissingValuePolicy(signal.Id.Value);

            Assert.IsInstanceOfType(result, typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
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
        public void MissingValuePolicyCanBeSetForSignal()
        {
            var signal1Id = AddNewIntegerSignal().Id.Value;
            var signal2Id = AddNewIntegerSignal().Id.Value;

            var policy1 = new Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<int>();
            var policy2 = new Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>()
            {
                Value = 42,
                Quality = Quality.Fair
            };

            client.SetMissingValuePolicy(signal1Id, policy1.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());
            client.SetMissingValuePolicy(signal2Id, policy2.ToDto<Dto.MissingValuePolicy.MissingValuePolicy>());

            var result1 = client.GetMissingValuePolicy(signal1Id);
            var result2 = client.GetMissingValuePolicy(signal2Id);

            Assert.IsInstanceOfType(result1, typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
            Assert.IsInstanceOfType(result2, typeof(Dto.MissingValuePolicy.SpecificValueMissingValuePolicy));

            var specificMissingValuePolicy = result2.ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<int>>();

            Assert.AreEqual(42, specificMissingValuePolicy.Value);
            Assert.AreEqual(Quality.Fair, specificMissingValuePolicy.Quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
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
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
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
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
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
            ForAllSignalTypes((dataType, granularity, quality, timestamp, message)
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

        private void WhenSettingSpecificValueMissingValuePolicy(DataType dataType, Quality quality)
        {
            var newPolicy = Domain.MissingValuePolicy.MissingValuePolicyBase.CreateForNativeType(
               typeof(Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<>),
               Domain.Infrastructure.DataTypeUtils.GetNativeType(dataType));

            var dtoPolicy = newPolicy.ToDto<Dto.MissingValuePolicy.SpecificValueMissingValuePolicy>();
            dtoPolicy.Quality = quality.ToDto<Dto.Quality>();

            client.SetMissingValuePolicy(signalId, dtoPolicy);
        }

        private void WhenSettingMissingValuePolicy(Type policyType, DataType signalDataType)
        {
            var newPolicy = Domain.MissingValuePolicy.MissingValuePolicyBase.CreateForNativeType(
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
