using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfLab.Tests.Infrastructure;
using WpfLab.Services;
using WpfLab.Tests.Mocks;
using WpfLab.ViewModel;

namespace WpfLab.Tests.Desktop.ViewModel
{
	[TestClass]
	public class ChildWindowViewModelFixture
	{
		private IUnityContainer _container;
		private ChildWindowViewModel _viewmodel;
		private IChildWindow _childWindow;
		private IChildWindowNested _childWindowNested;

		[TestInitialize]
		public void MyTestInitialize()
		{
			_container = UnityContainerResolver.Container;
			_childWindow = _container.Resolve<IChildWindow>();
			_childWindowNested = _container.Resolve<IChildWindowNested>();
			_viewmodel = new ChildWindowViewModel(_childWindow, _childWindowNested);
		}

		[TestCleanup]
		public void MyTestCleanup()
		{
			_container = null;
			_childWindow= null;
			_childWindow = null;
			_viewmodel = null;
		}
		[TestMethod]
		public void ChildWindowViewModel_OkCommand()
		{
			Assert.IsFalse(_childWindow.DialogResult.HasValue && _childWindow.DialogResult.Value);
			_viewmodel.OkCommand.Execute(null);
			Assert.IsTrue(_childWindow.DialogResult.HasValue && _childWindow.DialogResult.Value);
		}

		[TestMethod]
		public void ChildWindowViewModel_CancelCommand()
		{
			Assert.IsFalse(_childWindow.DialogResult.HasValue && _childWindow.DialogResult.Value);
			_viewmodel.CancelCommand.Execute(null);
			Assert.IsFalse(_childWindow.DialogResult.HasValue && _childWindow.DialogResult.Value);
		}

		[TestMethod]
		public void ChildWindowViewModel_OpenCommand()
		{
			Assert.IsFalse(_childWindow.DialogResult.HasValue && _childWindow.DialogResult.Value);
			_viewmodel.OpenCommand.Execute(null);
			Assert.IsNotNull(((MockChildWindowNested)_childWindowNested).Owner);
		}
	}
}
