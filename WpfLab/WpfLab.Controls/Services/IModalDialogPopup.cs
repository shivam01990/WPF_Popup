namespace WpfLab.Controls.services
{
    using System.Windows.Controls;

    public interface IModalDialogPopup
    {
        #region Properties

        /// <summary>
        /// Gets or sets the content to host
        /// </summary>
        Control HostedContent
        {
            get; set;
        }

        /// <summary>
        /// Shows the dialog
        /// </summary>
        bool IsOpen
        {
            get; set;
        }

        /// <summary>
        /// Gets the dialog title
        /// </summary>
        string Title
        {
            get;
            set;
        }

        #endregion Properties
    }
}