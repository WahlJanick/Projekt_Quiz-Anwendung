using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_Quiz_Anwendung.Classes;

namespace WPF_Quiz_Anwendung
{
    
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnThemeChanged(object sender, ThemeMode mode)
        {
            // Keine manuelle Anpassung erforderlich – DynamicResources aktualisieren automatisch.
            // Hier könntest du Zusatz-Effekte (z.B. Animation) einbauen.
        }
        
        public void LoadQuiz(object sender, RoutedEventArgs e)
        {
            try
            {
                string loadedFilePath;
                Quiz loadedQuiz = QuizFileHandler.LoadQuizFromFile(out loadedFilePath);
                if (loadedQuiz != null)
                {
                    NavigationService.Navigate(new QuestionPage(loadedQuiz, loadedFilePath));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Fehler beim Laden des Quiz: " + ex.Message);
            }
        }

        private void DefaultQuiz(object sender, RoutedEventArgs e)
        {
        }

        public void CreateQuiz(object sender, RoutedEventArgs e)
        {
            
        }

        private void SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
