
using WpfLab.Services;

namespace WpfLab.Tests.Mocks
{
public class MockChildWindow : IChildWindow
{
	public object Owner { get; set; }
	public void Close()
	{
		//
	}

	public bool? ShowDialog()
	{
		return true;
	}

	public void SetOwner(object window)
	{
		Owner = window;
	}

	public bool? DialogResult
	{
		get;
		set;
	}
}
}
