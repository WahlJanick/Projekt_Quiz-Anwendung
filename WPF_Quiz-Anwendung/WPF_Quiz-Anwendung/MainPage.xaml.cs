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


        private void LoadQuiz(object sender, RoutedEventArgs e)
        {
            Quiz loadedQuiz = QuizFileHandler.LoadQuizFromFile();
            if (loadedQuiz != null)
            {
                NavigationService.Navigate(new QuestionPage(loadedQuiz));
            }

        }

        private void OpenDefaultQuiz(object sender, RoutedEventArgs e)
        {
        }

        private void Navigate_CreateQuizPage(object sender, RoutedEventArgs e)
        {
            
        }

        private void Navigate_SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
