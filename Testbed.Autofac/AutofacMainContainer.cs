namespace Testbed.Autofac
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using global::Autofac;
	using global::Autofac.Builder;
	using global::Autofac.Extras.Multitenant;

	public class AutofacMainContainer : ContainerBuilder
	{
		public AutofacMainContainer()
		{
			this.RegisterType<ServiceConsumer>()
				.InstancePerDependency();


	
			this.RegisterType<Service>()
				.AsImplementedInterfaces()
				.InstancePerLifetimeScope();



			this.RegisterInstance<Func<string, IService>>(ServiceFactory.BuildEntity)
				.SingleInstance();
		}
	}
}
