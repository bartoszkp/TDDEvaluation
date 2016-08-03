using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Moq;
using Domain.Repositories;
using Dto.Conversions;
using DataAccess.GenericInstantiations;
using Domain.MissingValuePolicy;
using Domain;
using Dto;

namespace WebService.Tests
{
    class VerifyOrAssertTestResults
    {
        internal void GettingByPathAssertion(Dto.Signal result)
        {
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
            Assert.AreEqual(Dto.Granularity.Day, result.Granularity);
            CollectionAssert.AreEqual(new[] { "root", "signal1" }, result.Path.Components.ToArray());
        }

        internal void GettingByFalsePathAssertion(Dto.Path pathDto, ISignalsWebService signalsWebService)
        {
            try
            {
                signalsWebService.Get(pathDto);
            }
            catch (ArgumentException ae)
            {
                Assert.IsNotNull(ae);
                return;
            }
            Assert.Fail();
        }

        internal void AssertSetMissingValuePolicyIsExceptionThrownWhenInvalidKey(ISignalsWebService signalsWebService)
        {
            var policy = new Dto.MissingValuePolicy.SpecificValueMissingValuePolicy();
            try
            {
                signalsWebService.SetMissingValuePolicy(2, policy);
            }
            catch (KeyNotFoundException kne)
            {
                Assert.IsNotNull(kne);
                return;
            }
            Assert.Fail();
        }

        internal void AssertAddingASignalReturnsThisSignal(Dto.Signal result)
        {
            Assert.AreEqual(Dto.DataType.Decimal, result.DataType);
            Assert.AreEqual(Dto.Granularity.Week, result.Granularity);
            CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
        }

        internal void AssertRepositoryAddIsCalled(Mock<ISignalsRepository> signalsRepositoryMock)
        {
            signalsRepositoryMock.Verify(sr => sr.Add(It.Is<Domain.Signal>(passedSignal
                => passedSignal.DataType == Domain.DataType.Decimal
                    && passedSignal.Granularity == Domain.Granularity.Week
                    && passedSignal.Path.ToString() == "root/signal")));
        }

        internal void VerifyRepositorySetAndGetIsCalled(Mock<ISignalsRepository> signalsRepositoryMock, Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock)
        {
            signalsRepositoryMock.Verify(srm => srm.Get(1));
            missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
        }

        internal void VerifyRepositorySetAndGetIsCalled(Domain.Signal existingSignal,
            Mock<ISignalsRepository> signalsRepositoryMock,
            Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock)
        {
            signalsRepositoryMock.Verify(srm => srm.Get(1));
            missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.Is<Domain.Signal>(s =>
           (
               s.Id == existingSignal.Id
               && s.DataType == existingSignal.DataType
               && s.Granularity == existingSignal.Granularity
               && s.Path == existingSignal.Path
           )), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
        }

        internal void AssertGettingSignalsByItsId(int signalId, Dto.Signal result)
        {
            Assert.AreEqual(signalId, result.Id);
            Assert.AreEqual(Dto.DataType.String, result.DataType);
            Assert.AreEqual(Dto.Granularity.Year, result.Granularity);
            CollectionAssert.AreEqual(new[] { "root", "signal" }, result.Path.Components.ToArray());
        }

        internal void VerifyRepositorySetAndGetIsCalled(Domain.Signal existingSignal,
            Domain.MissingValuePolicy.MissingValuePolicyBase policy,
            Mock<ISignalsRepository> signalsRepositoryMock,
            Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock)
        {
            var specificPolicy = (Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>)policy;
            signalsRepositoryMock.Verify(srm => srm.Get(1));
            missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.Is<Domain.Signal>(s =>
           (
               s.Id == existingSignal.Id
               && s.DataType == existingSignal.DataType
               && s.Granularity == existingSignal.Granularity
               && s.Path == existingSignal.Path
           )), It.Is<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>(svmvp =>
           (
            svmvp.NativeDataType == policy.NativeDataType
            && svmvp.Quality == specificPolicy.Quality
            && svmvp.Value == specificPolicy.Value
           ))));
        }

        internal void AssertGetMissingValuePolicyIsExceptionThrownInvalidKey(ISignalsWebService signalsWebService)
        {
            try
            {
                signalsWebService.GetMissingValuePolicy(3).ToDomain<Domain.MissingValuePolicy.SpecificValueMissingValuePolicy<double>>();
            }
            catch (KeyNotFoundException kne)
            {
                Assert.IsNotNull(kne);
                return;
            }
            Assert.Fail();
        }

        internal void AssertGetIsReturningSpecificPolicy(SpecificValueMissingValuePolicyDouble existingPolicy, SpecificValueMissingValuePolicy<double> result)
        {
            Assert.AreEqual(existingPolicy.Id, result.Id);
            Assert.AreEqual(existingPolicy.Quality, result.Quality);
            Assert.AreEqual(existingPolicy.Value, result.Value);
        }

        internal void VerifyRepositoryGetIsCalled(Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock, Domain.Signal existingSignal)
        {
            missingValuePolicyRepositoryMock.Verify(mvp => mvp.Get(It.Is<Domain.Signal>(s =>
            (
                existingSignal.Id == existingSignal.Id
                && existingSignal.DataType == s.DataType
                && existingSignal.Granularity == s.Granularity
                && existingSignal.Path == s.Path
            ))));

        }
    }
}
