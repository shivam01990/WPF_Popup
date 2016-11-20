using System.ComponentModel;
using System.Windows;
using Microsoft.Practices.Unity;
using WpfLab.Infrastructure;
using WpfLab.Services;
using WpfLab.ViewModel;

namespace WpfLab.Views
{
	/// <summary>
	/// Interaction logic for ChildWindow.xaml
	/// </summary>
public partial class ChildWindow : Window, IChildWindow
{
	private readonly IUnityContainer _container;

	public ChildWindow()
	{
		InitializeComponent();
		_container = UnityContainerResolver.Container;
		var childWindowNested = _container.Resolve<IChildWindowNested>();

		DataContext = new ChildWindowViewModel(this, childWindowNested);
		Closing += ChildWindowClosing;
	}

	#region IChildWindow Members

	public void SetOwner(object window)
	{
		Owner = window as Window;
	}

	#endregion

	private void ChildWindowClosing(object sender, CancelEventArgs e)
	{
		e.Cancel = true;
		Visibility = Visibility.Hidden;
	}
}
}