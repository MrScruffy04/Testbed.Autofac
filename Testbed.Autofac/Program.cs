namespace Testbed.Autofac
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using global::Autofac;
	using global::Autofac.Extras.Multitenant;

	class Program
    {
        #region Main Program Loop

        private static ManualResetEvent _quitEvent = new ManualResetEvent(false);

        [STAThread]
        private static void Main(string[] args)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                _quitEvent.Set();
                e.Cancel = true;
            };

            try
            {
                #region Setup
                #endregion

				using (var container = new AutofacMainContainer().Build())
				{
					ProgramBody(container);
				}

                //  One of the following should be commented out. The other should be uncommented.

                //_quitEvent.WaitOne();  //  Wait on UI thread for Ctrl + C

                Console.ReadKey(true);  //  Wait for any character input
            }
            finally
            {
                #region Tear down
                #endregion
            }
        }

        #endregion




       
        private static void ProgramBody(IContainer container)
        {
			SimulateRequest(container);
		}

		private static void SimulateRequest(ILifetimeScope rootLifetimeScope)
		{
			using (var requestLifetimeScope = rootLifetimeScope.BeginLifetimeScope())
			{
				var consumer = requestLifetimeScope.Resolve<ServiceConsumer>();

				Console.WriteLine(consumer.DoSomething());
			}
		}

    }




	public interface IService
	{
		string DoSomething(string str);
	}

	public class Service : IService
	{
		private string _baseString = string.Empty;

		public void ConsumeBaseString(string baseString)
		{
			_baseString = baseString;
		}

		public string DoSomething(string str)
		{
			return string.Format("{0},{1}", _baseString, str);
		}
	}


	public class ServiceFactory
	{
		public static IService BuildEntity(string baseString)
		{
			var service = new Service();

			//  Manipulate service
			service.ConsumeBaseString(baseString);

			return service;
		}
	}





	public class ServiceConsumer
	{
		private readonly IService _service;

		public ServiceConsumer(Func<string, IService> serviceFactory)
		{
			_service = serviceFactory("AllYourBaseAre");
		}

		public string DoSomething()
		{
			//  This is an endpoint handler

			return _service.DoSomething("reticulating splines");
		}
	}
}
