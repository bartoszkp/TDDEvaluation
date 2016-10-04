using System;
using Domain;
using Dto.Conversions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignalsIntegrationTests.Infrastructure;

namespace SignalsIntegrationTests
{
    [TestClass]
    public class ShadowMissingValuePolicyAssignmentTests : TestsBase
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
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenYearASignal_WhenSettingShadowMissingValuePolicy_PolicyIsChangedForSignal()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, granularity);
            GivenASignalWith(dataType, granularity);

            WhenSettingShadowMissingValuePolicy(dataType, shadowSignal);

            ThenSignalHasShadowMissingValuePolicyWith(shadowSignal);
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondSignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteSignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourSignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADaySignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekSignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthSignal_WhenSettingYearShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Year);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingSecondShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Second);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingMinuteShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Minute);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingHourShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Hour);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingDayShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Day);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingWeekShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Week);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearSignal_WhenSettingMonthShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(dataType, Granularity.Month);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenASecondStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Second;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMinuteStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Minute;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAHourStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Hour;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenADayStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Day;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAWeekStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Week;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAMonthStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Month;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearBoolSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearBoolSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearBoolSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearBoolSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(bool).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearIntSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearIntSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearIntSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearIntSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(int).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDoubleSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDoubleSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDoubleSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDoubleSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(double).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDecimalSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDecimalSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDecimalSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearDecimalSignal_WhenSettingStringShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(decimal).FromNativeType();
            var shadowDataType = typeof(string).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearStringSignal_WhenSettingBoolShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(bool).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearStringSignal_WhenSettingIntShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(int).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearStringSignal_WhenSettingDoubleShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(double).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowMVP")]
        public void GivenAYearStringSignal_WhenSettingDecimalShadowMissingValuePolicy_ShouldThrow()
        {
            var granularity = Granularity.Year;
            var dataType = typeof(string).FromNativeType();
            var shadowDataType = typeof(decimal).FromNativeType();
            var shadowSignal = AddNewSignal(shadowDataType, granularity);
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, shadowSignal));
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenASecondBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Second, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenASecondIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Second, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenASecondDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Second, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenASecondDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Second, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenASecondStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Second, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMinuteBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Minute, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMinuteIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Minute, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMinuteDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Minute, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMinuteDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Minute, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMinuteStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Minute, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAHourBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Hour, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAHourIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Hour, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAHourDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Hour, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAHourDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Hour, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAHourStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Hour, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenADayBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Day, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenADayIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Day, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenADayDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Day, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenADayDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Day, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenADayStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Day, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAWeekBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Week, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAWeekIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Week, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAWeekDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Week, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAWeekDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Week, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAWeekStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Week, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMonthBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Month, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMonthIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Month, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMonthDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Month, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMonthDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Month, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAMonthStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Month, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAYearBooleanSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Year, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAYearIntegerSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Year, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAYearDoubleSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Year, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAYearDecimalSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Year, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenAYearStringSignal_WhenSettingItAsItsOwnShadow_ShouldThrow()
        {
            GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity.Year, DataType.String);
        }

        private void GivenASignal_WhenSettingItAsItsOwnShadow_ShouldThrow(Granularity granularity, DataType dataType)
        {
            GivenASignalWith(dataType, granularity);

            Assertions.AssertThrows(() => WhenSettingShadowMissingValuePolicy(dataType, client.GetById(signalId)));
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeSecondBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Second, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeSecondIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Second, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeSecondDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Second, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeSecondDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Second, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeSecondStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Second, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMinuteBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Minute, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMinuteIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Minute, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMinuteDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Minute, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMinuteDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Minute, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMinuteStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Minute, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeHourBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Hour, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeHourIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Hour, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeHourDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Hour, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeHourDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Hour, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeHourStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Hour, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeDayBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Day, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeDayIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Day, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeDayDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Day, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeDayDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Day, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeDayStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Day, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeWeekBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Week, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeWeekIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Week, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeWeekDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Week, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeWeekDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Week, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeWeekStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Week, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMonthBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Month, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMonthIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Month, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMonthDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Month, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMonthDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Month, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeMonthStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Month, DataType.String);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeYearBooleanSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Year, DataType.Boolean);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeYearIntegerSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Year, DataType.Integer);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeYearDoubleSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Year, DataType.Double);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeYearDecimalSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Year, DataType.Decimal);
        }

        [TestMethod]
        [TestCategory("issueShadowCycle")]
        public void GivenThreeYearStringSignals_WhenCreatingShadowDependencyCycle_ShouldThrow()
        {
            GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity.Year, DataType.String);
        }

        private void GivenThreeSignals_WhenCreatingShadowDependencyCycle_ShouldThrow(Granularity granularity, DataType dataType)
        {
            var signal1 = AddNewSignal(dataType, granularity);
            var signal2 = AddNewSignal(dataType, granularity);
            var signal3 = AddNewSignal(dataType, granularity);

            SetShadowMissingValuePolicy(signal1, signal2);
            SetShadowMissingValuePolicy(signal2, signal3);

            Assertions.AssertThrows(() => SetShadowMissingValuePolicy(signal3, signal1));
        }

        private static Domain.MissingValuePolicy.MissingValuePolicyBase CreateForNativeType(
            Type genericPolicyType,
            Type nativeType)
        {
            return genericPolicyType
                .MakeGenericType(nativeType)
                .GetConstructor(Type.EmptyTypes)
                .Invoke(null) as Domain.MissingValuePolicy.MissingValuePolicyBase;
        }

        private void SetShadowMissingValuePolicy(Dto.Signal signal, Dto.Signal shadowSignal)
        {
            var newPolicy = CreateForNativeType(
              typeof(Domain.MissingValuePolicy.ShadowMissingValuePolicy<>),
              Domain.Infrastructure.DataTypeUtils.GetNativeType(signal.DataType.ToDomain<Domain.DataType>()));

            var dtoPolicy = newPolicy.ToDto<Dto.MissingValuePolicy.ShadowMissingValuePolicy>();
            dtoPolicy.ShadowSignal = shadowSignal;

            client.SetMissingValuePolicy(signal.Id.Value, dtoPolicy);
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

        private void ThenSignalHasShadowMissingValuePolicyWith(Dto.Signal shadowSignal)
        {
            var policy = (Dto.MissingValuePolicy.ShadowMissingValuePolicy)client.GetMissingValuePolicy(signalId);
            Assert.AreEqual(shadowSignal.Id, policy.ShadowSignal.Id);
        }
    }
}
