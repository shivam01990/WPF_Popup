using Microsoft.Practices.Unity;
using WpfLab.Services;
using WpfLab.Views;

namespace WpfLab.Infrastructure
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
			_container.RegisterType<IMainWindow, MainWindow>();
			_container.RegisterType<IChildWindow, ChildWindow>();
			_container.RegisterType<IChildWindowNested, ChildWindowNested>();
		}
	}
}
