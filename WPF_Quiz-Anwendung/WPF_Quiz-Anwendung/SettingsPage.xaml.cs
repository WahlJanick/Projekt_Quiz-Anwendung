using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
            DefaultQuizTextBox.Text = "Standardfragen bearbeiten: " + Config.DefaultQuestionPath;
            NameTextBox.Text = "Benutzername ändern: " + Config.UserName;
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            Focusable = true;
            Focus();
            Keyboard.Focus(this);

            if (FindName("DarkModeToggle") is ToggleButton toggle)
            {
                toggle.IsChecked = Config.Theme == Theme.Dark;

                var app = (App)Application.Current;
                app.ThemeChanged += (_, theme) =>
                {
                    toggle.IsChecked = theme == Theme.Dark;
                };
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
            ((App)Application.Current).ApplyTheme(Theme.Dark);
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).ApplyTheme(Theme.Light);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

        private void DefaultQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "JSON Dateien (*.json)|*.json|Alle Dateien (*.*)|*.*",
                Title = "Standardquiz auswählen"
            };

            if (dlg.ShowDialog() == true)
                Config.DefaultQuestionPath = dlg.FileName;
            DefaultQuizTextBox.Text = "Standardfragen bearbeiten: " + Config.DefaultQuestionPath;
        }

        private void ListBoxItem_Selected(object sender, RoutedEventArgs e) 
        { 
            
        }

        private void UsernameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NameInputPage());
            NameTextBox.Text = "Benutzername ändern: " + Config.UserName;
        }
    }
}