using WpfLab.Controls.services;

namespace WpfLab.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Media;

    [TemplatePart(Name = "content", Type = typeof(Grid))]
    [TemplatePart(Name = "contentHost", Type = typeof(ContentPresenter))]
    [TemplatePart(Name = "dialog", Type = typeof(Popup))]
    public class ModalDialogPopup : ContentControl, IModalDialogPopup
    {
        #region Fields

        public static readonly DependencyProperty IsOpenProperty = 
           DependencyProperty.Register("IsOpen", typeof(bool), typeof(ModalDialogPopup),
           new PropertyMetadata(false, new PropertyChangedCallback(OnOpenChanged)));

        static AdornerLayer myAdorner;
        static List<ModalDialogPopup> popupList = new List<ModalDialogPopup>();
        static FrameworkElement rootElement;

        Button buttonCancel;
        Button buttonOK;
        Grid content;
        ContentPresenter contentHost;
        Popup dialog;
        bool flagIsLoaded;
        bool isOpenCache;
        double oldLeft = 0;
        double oldTop = 0;
        Window shell;
        TextBlock titleTextBlock;
        string _buttonCancelText = "Cancel";
        string _buttonOKText = "Ok";
        PopupAnimation _popupAnimation = PopupAnimation.Slide;
        Shader _shader = null;
        bool _showCancelButton = true;
        bool _showOKButton = true;

        #endregion Fields

        #region Constructors

        static ModalDialogPopup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalDialogPopup),
            new FrameworkPropertyMetadata(typeof(ModalDialogPopup)));
        }

        #endregion Constructors

        #region Events

        public event EventHandler Cancel;

        public event EventHandler Ok;

        #endregion Events

        #region Properties

        public string ButtonCancelText
        {
            get { return _buttonCancelText; }
            set { _buttonCancelText = value; }
        }

        public string ButtonOKText
        {
            get { return _buttonOKText; }
            set { _buttonOKText = value; }
        }

        public Control HostedContent
        {
            get;
            set;
        }

        public bool IsDesignMode
        {
            get
            {
                return DesignerProperties.GetIsInDesignMode(this);
            }
        }

        public bool IsOpen
        {
            get
            {
                return (bool)this.GetValue(IsOpenProperty);
            }
            set
            {
                this.SetValue(IsOpenProperty, value);
            }
        }

        public PopupAnimation PopupAnimation
        {
            get { return _popupAnimation; }
            set { _popupAnimation = value; }
        }

        public Shader Shader
        {
            get
            {
                if (_shader == null && !IsDesignMode)
                {
                    _shader = new Shader(rootElement);
                }
                return _shader;
            }
            set
            {
                _shader = value;
            }
        }

        public bool ShowCancelButton
        {
            get
            {
                return _showCancelButton;
            }
            set
            {
                _showCancelButton = value;
            }
        }

        public bool ShowOKButton
        {
            get
            {
                return _showOKButton;
            }
            set
            {
                _showOKButton = value;
            }
        }

        public string Title
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            dialog = GetTemplateChild("dialog") as Popup;
            content = GetTemplateChild("content") as Grid;
            contentHost = GetTemplateChild("contentHost") as ContentPresenter;
            buttonOK = GetTemplateChild("buttonOK") as Button;
            buttonCancel = GetTemplateChild("buttonCancel") as Button;
            titleTextBlock = GetTemplateChild("textBlockTitle") as TextBlock;

            if (dialog != null)
            {
                dialog.PopupAnimation = this.PopupAnimation;
                dialog.Closed += new System.EventHandler(dialog_Closed);
                Loaded += new RoutedEventHandler(ModalDialogHost_Loaded);
                Unloaded += new RoutedEventHandler(ModalDialogHost_Unloaded);
            }
            if (content != null)
            {
                content.Background = this.Background;
            }
            if (buttonOK != null)
            {
                if (ShowOKButton)
                {
                    buttonOK.Click += new RoutedEventHandler(buttonOK_Click);
                    buttonOK.Content = ButtonOKText;
                }
                buttonOK.Visibility = ConvertBoolToVisibility(ShowOKButton);
            }
            if (buttonCancel != null)
            {
                if (ShowCancelButton)
                {
                    buttonCancel.Click += new RoutedEventHandler(buttonCancel_Click);
                    buttonCancel.Content = ButtonCancelText;
                }
                buttonCancel.Visibility = ConvertBoolToVisibility(ShowCancelButton);
            }
            if (contentHost != null)
            {
                contentHost.Content = HostedContent;
            }
            if (titleTextBlock != null)
            {
                titleTextBlock.Text = Title;
            }
        }

        protected virtual void OnCancel(EventArgs e)
        {
            if (Cancel != null)
            {
                Cancel(this, e);
            }
        }

        protected virtual void OnOk(EventArgs e)
        {
            if (Ok != null)
            {
                Ok(this, e);
            }
        }

        private static void OnOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var context = (ModalDialogPopup)d;
            if (context.IsDesignMode)
            {
                //we don't want the popup to display above all windows
                //in vs.net designer when in designview
                return;
            }
            var isOpen = (bool)e.NewValue;

            //depdendency property changed callback fires
            //too soon, before OnApplyTemplate. so workaround :|
            if (context.flagIsLoaded == true)
            {
                if (isOpen)
                {
                    context.Show();
                }
                else
                {
                    context.Close();
                }
            }
            else if (isOpen)
            {
                context.isOpenCache = isOpen;
                context.IsOpen = !isOpen;
            }
        }

        void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Deque();
            OnCancel(EventArgs.Empty);
        }

        void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            Deque();
            OnOk(EventArgs.Empty);
        }

        void Close()
        {
            dialog.IsOpen = false;
			if (!IsDesignMode)
			{
				myAdorner.Visibility = Visibility.Hidden;
			}
        }

    	static Visibility ConvertBoolToVisibility(bool value)
        {
            return value ? Visibility.Visible : Visibility.Collapsed;
        }

        void Deque()
        {
            IsOpen = false;
            popupList.Remove(this);
            int count = popupList.Count;
            if (count == 0)
            {
                myAdorner.Visibility = Visibility.Hidden;
                return;
            }
            ModalDialogPopup top = popupList[count - 1];
            top.IsOpen = true;
        }

        void dialog_Closed(object sender, EventArgs e)
        {
        }

        void dialog_Opened(object sender, EventArgs e)
        {
            foreach (ModalDialogPopup p in popupList)
            {
                if (p != this && p.IsOpen)
                {
                    p.IsOpen = false;
                }
            }
        }

        void EnsureRootElement()
        {
            if (rootElement != null) return;

            rootElement = this.Parent as FrameworkElement;
            while (rootElement != null)
            {
                if (rootElement.Parent is Window)
                {
                    //we just want the direct child element of our window
                    //this is our root element.
                    break;
                }
                rootElement = rootElement.Parent as FrameworkElement;
            }
        }

        void ModalDialogHost_Loaded(object sender, RoutedEventArgs e)
        {
            flagIsLoaded = true;
            dialog.Opened += new EventHandler(dialog_Opened);
            dialog.Closed += new EventHandler(dialog_Closed);

            EnsureRootElement();
            if (!IsDesignMode)
            {
                if (shell == null)
                {
                    shell = ((Window)rootElement.Parent);
                    shell.LocationChanged += new System.EventHandler(Shell_LocationChanged);
                    shell.SizeChanged += new SizeChangedEventHandler(shell_SizeChanged);
                    shell.StateChanged += new EventHandler(shell_StateChanged);
                    shell.Deactivated += new EventHandler(shell_Deactivated);
                    shell.Activated += new EventHandler(shell_Activated);
                    oldLeft = shell.Left;
                    oldTop = shell.Top;
                }

                if (myAdorner == null)
                {
                    myAdorner = AdornerLayer.GetAdornerLayer(rootElement);
                    myAdorner.Visibility = Visibility.Hidden;
                    myAdorner.Add(this.Shader);
                }
            }
            //first set PlacementTarget and Placement
            if (rootElement != null)
            {
                dialog.PlacementTarget = rootElement;
                dialog.Placement = PlacementMode.Relative;
            }

            //and then only attempt to open and reposition, otherwise
            //only a portion of the dialog is rendered
            IsOpen = isOpenCache;
            if (IsOpen)
            {
                Show();
            }
        }

        void ModalDialogHost_Unloaded(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void Reposition()
        {
            EnsureRootElement();
            if (rootElement == null)
            {
                throw new Exception("ModalDialogPopup was unable to locate the root element.");
            }
            FrameworkElement elem = (FrameworkElement)dialog.Child;
            double actualX = (rootElement.ActualWidth / 2) - (elem.ActualWidth / 2);
            double actualY = (rootElement.ActualHeight / 2) - (elem.ActualHeight / 2);

            dialog.HorizontalOffset = Math.Abs(actualX);
            dialog.VerticalOffset = Math.Abs(actualY);
        }

        void shell_Activated(object sender, EventArgs e)
        {
            int count = popupList.Count;
            if (count > 0)
            {
                ModalDialogPopup host = popupList[count - 1];
                host.IsOpen = true;
            }
        }

        void shell_Deactivated(object sender, EventArgs e)
        {
            //popups by default are always above all windows
            //and when the main window loses focus
            //this looks awkward as well, so instead of pinvoke as
            //someone else is already doing here :
            //http://blogs.msdn.com/b/digitalnetbizz/archive/2007/01/11/hmmm-wpf-popup-is-always-top-most.aspx
            //we'll just hide it?
            int count = popupList.Count;
            if (count > 0)
            {
                ModalDialogPopup host = popupList[count - 1];
                host.IsOpen = false;
            }
        }

        void Shell_LocationChanged(object sender, System.EventArgs e)
        {
            Window s = (Window)sender;

            FrameworkElement elem = (FrameworkElement)dialog.Child;
            double actualX = (rootElement.ActualWidth / 2) - (elem.ActualWidth / 2);
            double actualY = (rootElement.ActualHeight / 2) - (elem.ActualHeight / 2);

            double x = Math.Abs(oldLeft - s.Left);
            double y = Math.Abs(oldTop - s.Top);

            dialog.HorizontalOffset = Math.Abs(x - actualX);
            dialog.VerticalOffset = Math.Abs(y - actualY);

            oldLeft = s.Left;
            oldTop = s.Top;
        }

        void shell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Window s = (Window)sender;
            Reposition();
        }

        void shell_StateChanged(object sender, EventArgs e)
        {
            Window s = (Window)sender;
            switch (s.WindowState)
            {
                case WindowState.Maximized:
                case WindowState.Normal:
                    if (IsOpen)
                    {
                        dialog.IsOpen = true;
                        Reposition();
                    }
                    break;
                case WindowState.Minimized:
                    if (IsOpen)
                    {
                        dialog.IsOpen = false;
                    }
                    break;
            }
        }

        void Show()
        {
            if (!popupList.Contains(this))
            {
                popupList.Add(this);
            }
            dialog.IsOpen = true;
            myAdorner.Visibility = Visibility.Visible;
            Reposition();
        }

        #endregion Methods
    }

    public class Shader : Adorner
    {
        #region Constructors

        public Shader(UIElement adornedElement)
            : base(adornedElement)
        {
            this.Background = new SolidColorBrush(Colors.Black);
            this.Background.Opacity = 0.5d;
            this.StrokeBorder = null;
        }

        public Shader(UIElement adornedElement, SolidColorBrush background, Pen strokeBorder)
            : this(adornedElement)
        {
            //caller needs to have set opacity on background brush
            this.Background = background;
            this.StrokeBorder = strokeBorder;
        }

        #endregion Constructors

        #region Properties

        SolidColorBrush Background
        {
            get;
            set;
        }

        Pen StrokeBorder
        {
            get;
            set;
        }

        #endregion Properties

        #region Methods

        protected override void OnRender(DrawingContext drawingContext)
        {
            FrameworkElement elem = (FrameworkElement)this.AdornedElement;
            Rect adornedElementRect = new Rect(0, 0, elem.ActualWidth, elem.ActualHeight);
            drawingContext.DrawRectangle(Background, StrokeBorder, adornedElementRect);
        }

        #endregion Methods
    }
}