using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace WPF_Quiz_Anwendung
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Focusable = true;
            Focus();
            Keyboard.Focus(this);

            var app = (App)Application.Current;
            if (FindName("DarkModeToggle") is ToggleButton toggle)
            {
                toggle.IsChecked = app.CurrentTheme == ThemeMode.Dark;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                if (NavigationService != null && NavigationService.CanGoBack)
                    NavigationService.GoBack();
                else
                    NavigationService?.Navigate(new MainPage());
                e.Handled = true;
            }
            base.OnPreviewKeyDown(e);
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).ApplyTheme(ThemeMode.Dark);
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).ApplyTheme(ThemeMode.Light);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }
        private void Button_Click(object sender, RoutedEventArgs e) { }
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e) { }
    }
}
