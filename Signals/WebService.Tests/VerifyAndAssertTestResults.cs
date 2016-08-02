using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Moq;
using Domain.Repositories;
using Dto.Conversions;

namespace WebService.Tests
{
    class VerifyAndAssertTestResults
    {
        public void GettingByPathAssertion(Dto.Signal result)
        {
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual(Dto.DataType.Boolean, result.DataType);
            Assert.AreEqual(Dto.Granularity.Day, result.Granularity);
            CollectionAssert.AreEqual(new[] { "root", "signal1" }, result.Path.Components.ToArray());
        }

        public void GettingByFalsePathAssertion(Dto.Path pathDto, ISignalsWebService signalsWebService)
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

        public void AssertSetMissingValuePolicyIsExceptionThrownWhenInvalidKey(ISignalsWebService signalsWebService)
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

        public void VerifyRepositorySetAndGetIsCalled(Mock<ISignalsRepository> signalsRepositoryMock, Mock<IMissingValuePolicyRepository> missingValuePolicyRepositoryMock)
        {
            signalsRepositoryMock.Verify(srm => srm.Get(1));
            missingValuePolicyRepositoryMock.Verify(mvp => mvp.Set(It.IsAny<Domain.Signal>(), It.IsAny<Domain.MissingValuePolicy.MissingValuePolicyBase>()));
        }

        public void VerifyRepositorySetAndGetIsCalled(Domain.Signal existingSignal,
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

        public void VerifyRepositorySetAndGetIsCalled(Domain.Signal existingSignal,
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

        public void AssertGetMissingValuePolicyIsExceptionThrownInvalidKey(ISignalsWebService signalsWebService)
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
    }
}
