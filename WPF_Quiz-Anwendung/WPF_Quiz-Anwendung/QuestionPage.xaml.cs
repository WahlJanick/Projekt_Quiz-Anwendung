using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_Quiz_Anwendung
{
    public partial class QuestionPage : Page
    {
        public QuestionPage()
        {
            InitializeComponent();
            Loaded += QuestionPage_Loaded;
        }

        private void QuestionPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Fokus sichern, damit ESC ankommt
            Focusable = true;
            Focus();
            Keyboard.Focus(this);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (NavigationService != null && NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
                else
                {
                    NavigationService?.Navigate(new MainPage());
                }
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // ggf. bestehende Logik
        }
    }
}
