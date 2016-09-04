using System;
using Domain;
using Domain.Infrastructure;
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

        private static Domain.MissingValuePolicy.MissingValuePolicyBase CreateForNativeType(
            Type genericPolicyType,
            Type nativeType)
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

        private void ThenSignalHasShadowMissingValuePolicyWith(Dto.Signal shadowSignal)
        {
            var policy = (Dto.MissingValuePolicy.ShadowMissingValuePolicy)client.GetMissingValuePolicy(signalId);
            Assert.AreEqual(shadowSignal.Id, policy.ShadowSignal.Id);
        }
    }
}
