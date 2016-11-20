using System.Windows.Input;
using WpfLab.Infrastructure;
using WpfLab.Services;

namespace WpfLab.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly IChildWindow _childWindow;
		private readonly IMainWindow _mainWindow;
		private ICommand _cancelCommand;
		private bool _firstPopupIsOpen;
		private ICommand _okCommand;
		private ICommand _openCommand;
		private ICommand _openFirstPopupCommand;
		private ICommand _openSecondPopupCommand;

		private bool _secondPopupIsOpen;

		public MainWindowViewModel(IMainWindow mainWindow, IChildWindow childWindow)
		{
			_mainWindow = mainWindow;
			_childWindow = childWindow;
		}

		public bool FirstPopupIsOpen
		{
			get { return _firstPopupIsOpen; }
			set
			{
				_firstPopupIsOpen = value;
				OnPropertyChanged("FirstPopupIsOpen");
			}
		}

		public bool SecondPopupIsOpen
		{
			get { return _secondPopupIsOpen; }
			set
			{
				_secondPopupIsOpen = value;
				OnPropertyChanged("SecondPopupIsOpen");
			}
		}

		public ICommand OpenCommand
		{
			get { return (_openCommand = _openCommand ?? new DelegateCommand(OpenClick)); }
		}


		public ICommand OpenFirstPopupCommand
		{
			get { return _openFirstPopupCommand ?? (_openFirstPopupCommand = new DelegateCommand(OpenFirstPopupClick)); }
		}

		public ICommand OpenSecondPopupCommand
		{
			get { return _openSecondPopupCommand ?? ( _openSecondPopupCommand = new DelegateCommand(OpenSecondPopupClick)); }
		}


		public ICommand OkCommand
		{
			get { return _okCommand ?? ( _okCommand = new DelegateCommand(OkClick)); }
		}

		public ICommand CancelCommand
		{
			get { return _cancelCommand ?? ( _cancelCommand = new DelegateCommand(CancelClick)); }
		}

		private void OpenFirstPopupClick()
		{
			FirstPopupIsOpen = true;
		}

		private void OpenSecondPopupClick()
		{
			SecondPopupIsOpen = true;
		}

		private void OpenClick()
		{
			_childWindow.SetOwner(_mainWindow);
			var result = _childWindow.ShowDialog();
		}

		private void CancelClick()
		{
			_mainWindow.Close();
		}

		private void OkClick()
		{
			_mainWindow.Close();
		}
	}
}