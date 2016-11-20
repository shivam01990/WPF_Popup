using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfLab.Tests.Infrastructure;
using WpfLab.Services;
using WpfLab.ViewModel;

namespace WpfLab.Tests.Desktop.ViewModel
{
	[TestClass]
	public class ChildWindowNestedViewModelFixture
	{
		private IUnityContainer _container;
		private ChildWindowNestedViewModel _viewmodel;
		private IChildWindowNested _childWindowNested;

		[TestInitialize]
		public void MyTestInitialize()
		{
			_container = UnityContainerResolver.Container;
			_childWindowNested = _container.Resolve<IChildWindowNested>();
			_viewmodel = new ChildWindowNestedViewModel(_childWindowNested);
		}

		[TestCleanup]
		public void MyTestCleanup()
		{
			_container = null;
			_childWindowNested = null;
			_viewmodel = null;
		}

		[TestMethod]
		public void ChildWindowNestedViewModel_OkCommand()
		{
			Assert.IsFalse(_childWindowNested.DialogResult.HasValue && _childWindowNested.DialogResult.Value);
			_viewmodel.OkCommand.Execute(null);
			Assert.IsTrue(_childWindowNested.DialogResult.HasValue && _childWindowNested.DialogResult.Value);
		}

		[TestMethod]
		public void ChildWindowNestedViewModel_CancelCommand()
		{
			Assert.IsFalse(_childWindowNested.DialogResult.HasValue && _childWindowNested.DialogResult.Value);
			_viewmodel.CancelCommand.Execute(null);
			Assert.IsFalse(_childWindowNested.DialogResult.HasValue && _childWindowNested.DialogResult.Value);
		}
	}
}
