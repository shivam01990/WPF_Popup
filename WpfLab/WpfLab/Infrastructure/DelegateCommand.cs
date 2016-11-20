using System;
using System.Windows.Input;

namespace WpfLab.Infrastructure
{
	public class DelegateCommand : ICommand
	{
		private readonly Predicate<object> _canExecute;
		private readonly Action _execute;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action execute)
			: this(execute, null)
		{
		}

		public DelegateCommand(Action execute,
					   Predicate<object> canExecute)
		{
			_execute = execute;
			_canExecute = canExecute;
		}

		public virtual bool CanExecute(object parameter)
		{
			return _canExecute == null || _canExecute(parameter);
		}

		public virtual void Execute()
		{
			_execute();
		}

		public virtual void Execute(object parameter)
		{
			//this version doesn't support parameters
			_execute();
		}

		public void RaiseCanExecuteChanged()
		{
			if (CanExecuteChanged != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}
