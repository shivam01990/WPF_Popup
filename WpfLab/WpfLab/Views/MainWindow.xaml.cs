using System;
using System.Windows;
using Microsoft.Practices.Unity;
using WpfLab.Infrastructure;
using WpfLab.Services;
using WpfLab.ViewModel;

namespace WpfLab.Views
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	/// 
	public partial class MainWindow : Window, IMainWindow
	{
		private readonly IUnityContainer _container;
		public MainWindow()
		{
			InitializeComponent();
			_container = UnityContainerResolver.Container;
			var childWindow = _container.Resolve<IChildWindow>();

			DataContext = new MainWindowViewModel(this, childWindow);
		}

		public void SetOwner(object window)
		{
			Owner = window as Window;
		}
	}
}
