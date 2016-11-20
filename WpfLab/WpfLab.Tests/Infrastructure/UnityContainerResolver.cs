using Microsoft.Practices.Unity;
using WpfLab.Services;
using WpfLab.Tests.Mocks;
using WpfLab.Views;

namespace WpfLab.Tests.Infrastructure
{
	public class UnityContainerResolver
	{
		private static IUnityContainer _container;

		private UnityContainerResolver()
		{
		}

		public static IUnityContainer Container
		{
			get
			{
				if (_container == null)
				{
					_container = new UnityContainer();
					RegisterTypes();
				}
				return _container;
			}
		}

		static void RegisterTypes()
		{
			_container.RegisterType<IMainWindow, MockMainWindow>();
			_container.RegisterType<IChildWindow, MockChildWindow>();
			_container.RegisterType<IChildWindowNested, MockChildWindowNested>();
		}
	}
}
