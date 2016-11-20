
namespace WpfLab.Services
{
	public interface IChildWindowNested
	{
		void Close();
		bool? ShowDialog();
		void SetOwner(object window);
		bool? DialogResult { get; set; }
	}
}
