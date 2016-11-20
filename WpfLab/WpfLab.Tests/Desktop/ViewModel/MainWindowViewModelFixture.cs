using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfLab.Tests.Infrastructure;
using WpfLab.Services;
using WpfLab.ViewModel;

namespace WpfLab.Tests.Desktop.ViewModel
{
	[TestClass]
	public class MainWindowViewModelFixture
	{
		private IUnityContainer _container;
		private MainWindowViewModel _viewmodel;
		private IMainWindow _mainWindow;
		private IChildWindow _childWindow;

		[TestInitialize]
		public void MyTestInitialize()
		{
			_container = UnityContainerResolver.Container;
			_mainWindow = _container.Resolve<IMainWindow>();
			_childWindow = _container.Resolve<IChildWindow>();
			_viewmodel = new MainWindowViewModel(_mainWindow, _childWindow);
		}

		[TestCleanup]
		public void MyTestCleanup()
		{
			_container = null;
			_mainWindow = null;
			_viewmodel = null;
		}
		
		[TestMethod]
		public void MainWindowViewModel_OkCommand()
		{
			Assert.IsFalse(_mainWindow.DialogResult.HasValue && _mainWindow.DialogResult.Value);
			_viewmodel.OkCommand.Execute(null);
			Assert.IsTrue(_mainWindow.DialogResult.HasValue && _mainWindow.DialogResult.Value);
		}

		[TestMethod]
		public void MainWindowViewModel_CancelCommand()
		{
			Assert.IsFalse(_mainWindow.DialogResult.HasValue && _mainWindow.DialogResult.Value);
			_viewmodel.CancelCommand.Execute(null);
			Assert.IsTrue(_mainWindow.DialogResult.HasValue && _mainWindow.DialogResult.Value);
		}

		#region ModalPopup Tests
		[TestMethod]
		public void MainWindowViewModel_OpenFirstPopupCommand()
		{
			Assert.IsFalse(_viewmodel.FirstPopupIsOpen);
			_viewmodel.OpenFirstPopupCommand.Execute(null);
			Assert.IsTrue(_viewmodel.FirstPopupIsOpen);
		}

		[TestMethod]
		public void MainWindowViewModel_OpenSecondPopupCommand()
		{
			Assert.IsFalse(_viewmodel.SecondPopupIsOpen);
			_viewmodel.OpenSecondPopupCommand.Execute(null);
			Assert.IsTrue(_viewmodel.SecondPopupIsOpen);
		}
		#endregion
	}
}
