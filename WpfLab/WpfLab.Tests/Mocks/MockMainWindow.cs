
using WpfLab.Services;

namespace WpfLab.Tests.Mocks
{
	public class MockMainWindow : IMainWindow
	{
		public void Close()
		{
			DialogResult = true;
		}

		public bool? DialogResult
		{
			get; 
			set; 
		}
	}
}
