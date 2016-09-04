using System;
using Domain;
using Domain.Infrastructure;
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
        public void GivenASecondBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMinuteBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAHourBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenADayBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAWeekBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMonthBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAYearBoolSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(bool).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenASecondIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMinuteIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAHourIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenADayIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAWeekIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMonthIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAYearIntSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(int).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenASecondDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMinuteDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAHourDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenADayDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAWeekDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMonthDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAYearDoubleSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(double).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenASecondDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMinuteDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAHourDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenADayDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAWeekDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMonthDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAYearDecimalSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(decimal).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenASecondStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Second;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMinuteStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Minute;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAHourStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Hour;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenADayStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Day;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAWeekStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Week;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAMonthStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Month;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue5")]
        public void GivenAYearStringSignal_WhenGettingMissingValuePolicy_NoneQualityMissingValuePolicyIsReturned()
        {
            var granularity = Granularity.Year;
            GivenASignalWith(typeof(string).FromNativeType(), granularity);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
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
        public void GivenASecondBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingNoneMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);
            WithNonDefaultMissingValuePolicyFor(dataType);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.NoneQualityMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.NoneQualityMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingSpecificValueMissingValuePolicyWithBadQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Bad;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingSpecificValueMissingValuePolicyWithPoorQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Poor;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingSpecificValueMissingValuePolicyWithFairQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Fair;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingSpecificValueMissingValuePolicyWithGoodQuality_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var quality = Quality.Good;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingSpecificValueMissingValuePolicy(dataType, quality);

            ThenSignalHasSpecificMissingValuePolicyWith(quality);
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearBoolSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearStringSignal_WhenSettingZeroOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.ZeroOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.ZeroOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearIntSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDoubleSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenASecondDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMinuteDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAHourDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenADayDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAWeekDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAMonthDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        [TestCategory("issue3")]
        public void GivenAYearDecimalSignal_WhenSettingFirstOrderMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            GivenASignalWith(dataType, granularity);

            WhenSettingMissingValuePolicy(
                typeof(Domain.MissingValuePolicy.FirstOrderMissingValuePolicy<>),
                dataType);

            ThenMissingValuePolicyHasType(typeof(Dto.MissingValuePolicy.FirstOrderMissingValuePolicy));
        }

        [TestMethod]
        // TODO [TestCategory("issueXX")]
        // TODO multiply
        public void GivenASignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        // TODO shadow with wrong signal granularity
        // TODO shadow with wrong signal type

        private static Domain.MissingValuePolicy.MissingValuePolicyBase CreateForNativeType(
            Type genericPolicyType, Type nativeType)
        {
            return genericPolicyType
                .MakeGenericType(nativeType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null) as Domain.MissingValuePolicy.MissingValuePolicyBase;
        }

        private void WhenSettingShadowMissingValuePolicy(DataType dataType, Dto.Signal shadowSignal)
        {
            var newPolicy = CreateForNativeType(
               typeof(Domain.MissingValuePolicy.ShadowMissingValuePolicy<>),
               Domain.Infrastructure.DataTypeUtils.GetNativeType(dataType));

            var dtoPolicy = newPolicy.ToDto<Dto.MissingValuePolicy.ShadowMissingValuePolicy>();
            dtoPolicy.ShadowSignal = shadowSignal;

            client.SetMissingValuePolicy(signalId, dtoPolicy);
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

        private void ThenSignalHasShadowMissingValuePolicyWith(Dto.Signal shadowSignal)
        {
            var policy = (Dto.MissingValuePolicy.ShadowMissingValuePolicy)client.GetMissingValuePolicy(signalId);
            Assert.AreEqual(shadowSignal.Id, policy.ShadowSignal.Id);
        }
    }
}
