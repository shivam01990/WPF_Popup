
namespace WpfLab.Services
{
	public interface IMainWindow
	{
		void Close();
		bool? DialogResult { get; set; }
	}
}
