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
            NavigationService.Navigate(new QuestionPage());
        }

        private void OpenDefaultQuiz(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuestionPage());
        }

        private void Navigate_CreateQuizPage(object sender, RoutedEventArgs e)
        {
            var quiz = new WPF_Quiz_Anwendung.Classes.Quiz(
                "Allgemeinwissen",
                new List<WPF_Quiz_Anwendung.Classes.Question>
                {
                new WPF_Quiz_Anwendung.Classes.Question(
                "Was ist die Hauptstadt von Deutschland?",
                    WPF_Quiz_Anwendung.Classes.QuestionType.SingleRightAnswer,
                    new List<WPF_Quiz_Anwendung.Classes.Answer>
                    {
                        new WPF_Quiz_Anwendung.Classes.Answer("Berlin", true),
                        new WPF_Quiz_Anwendung.Classes.Answer("München", false),
                        new WPF_Quiz_Anwendung.Classes.Answer("Hamburg", false)
                    }
                ),
                new WPF_Quiz_Anwendung.Classes.Question(
                    "Welche Farben hat die deutsche Flagge?",
                    WPF_Quiz_Anwendung.Classes.QuestionType.MultipleRightAnswers,
                    new List<WPF_Quiz_Anwendung.Classes.Answer>
                    {
                        new WPF_Quiz_Anwendung.Classes.Answer("Schwarz", true),
                        new WPF_Quiz_Anwendung.Classes.Answer("Rot", true),
                        new WPF_Quiz_Anwendung.Classes.Answer("Gelb", true),
                        new WPF_Quiz_Anwendung.Classes.Answer("Grün", false)
                    }
                )
                }
            );
            QuizFileHandler.SaveQuizToFile(quiz);
        }

        private void Navigate_SettingsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SettingsPage());
        }
    }
}
