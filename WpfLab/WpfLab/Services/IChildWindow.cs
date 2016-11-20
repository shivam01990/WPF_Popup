
namespace WpfLab.Services
{
	public interface IChildWindow
	{
		void Close();
		bool? ShowDialog();
		void SetOwner(object window);
		bool? DialogResult { get; set; }
	}
}
