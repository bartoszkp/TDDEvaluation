using System;
using DataAccess;
using Microsoft.Practices.Unity;
using PostSharp.Aspects;

namespace WebService.Infrastructure
{
    [Serializable]
    public class DatabaseTransaction : PostSharp.Aspects.OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            var unitOfWorkProvider = GetUnitOfWorkProvider(args.Instance);

            unitOfWorkProvider.OpenUnitOfWork();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var unitOfWorkProvider = GetUnitOfWorkProvider(args.Instance);

            if (args.Exception == null && unitOfWorkProvider.CurrentUnitOfWork != null)
            {
                unitOfWorkProvider.CurrentUnitOfWork.Commit();
            }

            if (unitOfWorkProvider.CurrentUnitOfWork != null)
            {
                unitOfWorkProvider.CurrentUnitOfWork.Dispose();
            }
        }

        private static IUnitOfWorkProvider GetUnitOfWorkProvider(object instance)
        {
            var unityContainerProperty = instance.GetType().GetProperty("UnityContainer");
            var unityContainer = unityContainerProperty.GetValue(instance) as IUnityContainer;

            if (unityContainer == null)
            {
                throw new InvalidOperationException("Type using DatabaseTransaction aspect must provide UnityContainer property");
            }

            return unityContainer.Resolve<IUnitOfWorkProvider>();
        }
    }
}
