using System;
using System.Windows;
using Microsoft.Practices.Unity;
using WpfLab.Infrastructure;
using WpfLab.Services;
using WpfLab.ViewModel;

namespace WpfLab.Views
{
	/// <summary>
	/// Interaction logic for ChildWindowNested.xaml
	/// </summary>
	public partial class ChildWindowNested : Window, IChildWindowNested
	{
		private readonly IUnityContainer _container;
		public ChildWindowNested()
		{
			InitializeComponent();
			_container = UnityContainerResolver.Container;

			DataContext = new ChildWindowNestedViewModel(this);
			Closing += ChildWindowNestedClosing;
		}

		void ChildWindowNestedClosing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			Visibility = Visibility.Hidden;
		}

		public void SetOwner(object window)
		{
			Owner = window as Window;
		}
	}
}
