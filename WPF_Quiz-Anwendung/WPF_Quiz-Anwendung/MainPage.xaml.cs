using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_Quiz_Anwendung
{
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            ((App)Application.Current).ThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(object sender, ThemeMode mode)
        {
            // Keine manuelle Anpassung erforderlich – DynamicResources aktualisieren automatisch.
            // Hier könntest du Zusatz-Effekte (z.B. Animation) einbauen.
        }

        private void LoadQuiz(object sender, RoutedEventArgs e) { }

        private void OpenDefaultQuiz(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuestionPage());
        }

        private void Navigate_CreateQuizPage(object sender, RoutedEventArgs e) { }

        private void Navigate_SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
