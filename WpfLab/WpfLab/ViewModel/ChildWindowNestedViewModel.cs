
using System.Windows.Input;
using WpfLab.Infrastructure;
using WpfLab.Services;

namespace WpfLab.ViewModel
{
	public class ChildWindowNestedViewModel
	{
		private readonly IChildWindowNested _childWindowNested;

		public ChildWindowNestedViewModel(IChildWindowNested childWindowNested)
		{
			_childWindowNested = childWindowNested;
		}

		private ICommand _okCommand;
		public ICommand OkCommand
		{
			get { return _okCommand ?? ( _okCommand = new DelegateCommand(OkClick)); }
		}

		private ICommand _cancelCommand;
		public ICommand CancelCommand
		{
			get { return _cancelCommand ?? ( _cancelCommand = new DelegateCommand(CancelClick)); }
		}

		private void CancelClick()
		{
			_childWindowNested.DialogResult = false;
			_childWindowNested.Close();
		}

		private void OkClick()
		{
			_childWindowNested.DialogResult = true;
			_childWindowNested.Close();
		}


	}
}
